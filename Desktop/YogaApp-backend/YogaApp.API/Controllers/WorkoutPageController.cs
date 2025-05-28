using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileYogaApp.Extensions;
using MobileYogaApp.Presentation.ViewModels;
using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Application.ServiceContact.Workout;

namespace MobileYogaApp.Controllers;

[Authorize]
[ApiController]
[Route("workouts")]
public class WorkoutPageController : ControllerBase
{
    private readonly IWorkoutService _workoutService;
    private readonly IUserService _userService;
    private readonly ILogger<WorkoutPageController> _logger;

    public WorkoutPageController(IWorkoutService workoutService, IUserService userService,
        ILogger<WorkoutPageController> logger)
    {
        _logger = logger;
        _logger.LogInformation("WorkoutPageController initialized");
        _workoutService = workoutService;
        _userService = userService;
    }
    [HttpGet("all-workouts")]
    public async Task<IActionResult> GetAllWorkouts()
    {
        var userId = User.GetUserId();

        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to GetAllWorkouts: User ID not found in token");
            return Unauthorized("User ID not found in token");
        }

        _logger.LogInformation("User {UserId} is requesting all workouts", userId);
        var user = await _userService.GetUserByIdAsync(userId.Value);
        var userName = user.Name;

        var homePageModel = new WorkoutPageModel
        {
            UserName = userName,
            WorkoutsByCategory = new Dictionary<string, List<WorkoutPreviewDto>?>
            {
                { "Beginner", await _workoutService.GetWorkoutsByCategoryPreviewAsync("Beginner") },
                { "Intermediate", await _workoutService.GetWorkoutsByCategoryPreviewAsync("Intermediate")},
                { "Advanced", await _workoutService.GetWorkoutsByCategoryPreviewAsync("Advanced") },
                { "Trending", await _workoutService.GetWorkoutsByCategoryPreviewAsync("Trending") }
            }
        };

        return Ok(homePageModel);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchWorkouts(string query)
    {
        _logger.LogInformation("SearchWorkouts called with query: {Query}", query);

        var workouts = await _workoutService.SearchWorkoutsByNameAsync(query);
        _logger.LogInformation("Found {Count} workouts matching query: {Query}", workouts.Count(), query);
        
        return Ok(workouts);
    }

    [HttpGet("favourite-workouts")]
    public async Task<IActionResult> GetFavouriteWorkouts()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access to GetFavouriteWorkouts");
            return Unauthorized("User ID not found in token");
        }

        _logger.LogInformation("Fetching favourite workouts for user {UserId}", userId);
        var favouriteWorkouts = await _workoutService.GetFavouriteWorkoutsAsync(userId.Value);
        return Ok(favouriteWorkouts);
    }

    [HttpGet("recent-workouts")]
    public async Task<IActionResult> GetRecentWorkouts()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access to GetRecentWorkouts");
            return Unauthorized("User ID not found in token");
        }

        _logger.LogInformation("Fetching recent workouts for user {UserId}", userId);
        var recentWorkouts = await _workoutService.GetRecentWorkoutsAsync(userId);
        return Ok(recentWorkouts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkoutDetails(Guid id)
    {
        _logger.LogInformation("Fetching workout details for ID: {WorkoutId}", id);
        var workout = await _workoutService.GetWorkoutByIdAsync(id);
        if (workout == null)
        {
            _logger.LogWarning("Workout with ID {WorkoutId} not found", id);
            throw new KeyNotFoundException("Workout not found");
        }

        return Ok(workout);
    }

    [HttpGet("start-workout/{workoutId}")]
    public async Task<IActionResult> StartWorkout(Guid workoutId)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("User {UserId} starting workout {WorkoutId}", userId, workoutId);
        await _workoutService.StartWorkoutAsync(userId, workoutId);
        return Ok();
    }

    [HttpGet("finish-workout/{workoutId}")]
    public async Task<IActionResult> FinishWorkout(Guid workoutId)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("User {UserId} finishing workout {WorkoutId}", userId, workoutId);
        await _workoutService.FinishWorkoutAsync(userId, workoutId);
        return Ok();
    }

    [HttpPost("add-favourite-workout")]
    public async Task<IActionResult> AddFavouriteWorkout([FromBody] Guid workoutId)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access to AddFavouriteWorkout");
            return Unauthorized("User ID not found in token");
        }

        _logger.LogInformation("User {UserId} adding workout {WorkoutId} to favourites", userId, workoutId);
        var result = await _workoutService.AddWorkoutToFavouritesAsync(userId.Value, workoutId);
        if (!result)
        {
            _logger.LogError("Failed to add workout {WorkoutId} to favourites for user {UserId}", workoutId, userId);
            return BadRequest("Failed to add workout to favourites.");
        }

        return Ok("Workout added to favourites.");
    }

    [HttpPost("add-workout")]
    public async Task<IActionResult> CreateWorkout([FromBody] WorkoutRequestDto request)
    {
        _logger.LogInformation("Creating new workout with name: {Name}", request.Name);
        var createdWorkout = await _workoutService.CreateWorkoutAsync(request);
        _logger.LogInformation("Workout created with ID: {WorkoutId}", createdWorkout.WorkoutId);
        return Ok(new { createdWorkout.WorkoutId });
    }

    [HttpPost("{workoutId}/add-exercise")]
    public async Task<IActionResult> AddExerciseToWorkout(Guid workoutId, [FromBody] AddExerciseToWorkoutDto? request)
    {
        if (request == null || request.Exercise == null)
        {
            _logger.LogWarning("AddExerciseToWorkout failed: Exercise data is missing");
            return BadRequest("Exercise data is required.");
        }

        _logger.LogInformation("Adding exercise to workout {WorkoutId}", workoutId);
        var success = await _workoutService.AddExerciseToWorkoutAsync(workoutId, request.Exercise, request.ExistingExerciseName);
        if (!success)
        {
            _logger.LogError("Failed to add exercise to workout {WorkoutId}", workoutId);
            return BadRequest("Failed to add exercise");
        }

        return Ok("Exercise added to workout");
    }

    [HttpPut("update-workout/{id}")]
    public async Task<IActionResult> UpdateWorkout(Guid id, [FromBody] WorkoutRequestDto? request)
    {
        if (request == null)
        {
            _logger.LogWarning("UpdateWorkout failed: Workout data is missing");
            return BadRequest("Workout data is required.");
        }

        _logger.LogInformation("Updating workout {WorkoutId}", id);
        var updatedWorkout = await _workoutService.UpdateWorkoutAsync(id, request);
        return Ok(updatedWorkout);
    }

    [HttpDelete("delete-workout/{id}")]
    public async Task<IActionResult> DeleteWorkout(Guid id)
    {
        _logger.LogInformation("Deleting workout {WorkoutId}", id);
        var deletedWorkout = await _workoutService.DeleteWorkoutAsync(id);
        return Ok(deletedWorkout);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileYogaApp.Extensions;
using MobileYogaApp.Presentation.ViewModels;
using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.Workout;

namespace MobileYogaApp.Controllers;

[Authorize]
[ApiController]
[Route("custom-workouts")]
public class CustomWorkoutPageController : ControllerBase
{
    private readonly ICustomWorkoutService _customWorkoutService;
    private readonly ILogger<CustomWorkoutPageController> _logger;

    public CustomWorkoutPageController(ICustomWorkoutService customWorkoutService,
        ILogger<CustomWorkoutPageController> logger)
    {
        _customWorkoutService = customWorkoutService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomWorkoutPreviewsAsync()
    {
        _logger.LogInformation("Gett all custom workouts");
        var workouts = await _customWorkoutService.GetAllCustomWorkoutPreviewsAsync();
        var pageModel = new CustomWorkoutPageModel
        {
            Title = "Custom Workouts",
            CustomWorkouts = workouts
        };
        return Ok(pageModel);
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchCustomWorkouts(string query)
    {
        var workouts = await _customWorkoutService.SearchWorkoutsByNameAsync(query);
        return Ok(workouts);
    }

    [HttpGet("{customWorkoutId}")]
    public async Task<IActionResult> GetCustomWorkoutDetails(Guid customWorkoutId)
    {
        _logger.LogInformation("Getting custom workout with ID: {CustomWorkoutId}", customWorkoutId);
        
        var workout = await _customWorkoutService.GetWorkoutByIdAsync(customWorkoutId);
        if (workout == null)
        {
            _logger.LogWarning("Custom workout with ID {CustomWorkoutId} not found", customWorkoutId);
            return NotFound($"Custom workout with ID {customWorkoutId} not found.");
        }

        return Ok(workout);
    }

    [HttpPost("add_custom-workout")]
    public async Task<IActionResult> CreateCustomWorkoutAsync([FromBody] CustomWorkoutRequestDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var userId = User.GetUserId();
        if (userId == null)
        {
            _logger.LogWarning("User ID not found in claims");
            return Unauthorized("User ID not found in claims");
        }
        
        requestDto.UserId = userId;

        var createdWorkout = await _customWorkoutService.CreateWorkoutAsync(requestDto);
        _logger.LogInformation("Воркаут створено з ID: {CustomWorkoutId}", createdWorkout.CustomWorkoutId);
        
        return Ok(createdWorkout);
    }
    
    [HttpPost("{customWorkoutId}/add-exercise")]
    public async Task<IActionResult> AddExerciseToCustomWorkout(Guid customWorkoutId, [FromBody] AddExerciseToWorkoutDto? request)
    {
        if (request == null || request.Exercise == null)
        {
            _logger.LogWarning("Try to add empty exercise to custom workout {CustomWorkoutId}", customWorkoutId);
            return BadRequest("Exercise data is required.");
        }

        var success = await _customWorkoutService.AddExerciseToWorkoutAsync(customWorkoutId, request.Exercise, request.ExistingExerciseName);
        if (!success)
        {
            _logger.LogWarning("Failed to add exercise {ExerciseName} to custom workout {CustomWorkoutId}",
                request.Exercise.Name, customWorkoutId);
            return BadRequest("Failed to add exercise");
        }

        _logger.LogInformation("Exercise {ExerciseName} added to custom workout {CustomWorkoutId}", request.Exercise.Name, customWorkoutId);
        return Ok("Exercise added to custom workout");
    }
    
    [HttpPut("{customWorkoutId}/update-custom-workout")]
    public async Task<IActionResult> UpdateCustomWorkout(Guid customWorkoutId, [FromBody] CustomWorkoutRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("Updating custom workout with ID: {CustomWorkoutId}", customWorkoutId);
        var updatedWorkout = await _customWorkoutService.UpdateWorkoutAsync(customWorkoutId, request);
        return Ok(updatedWorkout);
    }
    
    [HttpDelete("{customWorkoutId}/delete-custom-workout")]
    public async Task<IActionResult> DeleteCustomWorkout(Guid customWorkoutId)
    {
        _logger.LogInformation("Deleting custom workout with ID: {CustomWorkoutId}", customWorkoutId);
        var deletedWorkout = await _customWorkoutService.DeleteWorkoutAsync(customWorkoutId);
        return Ok(deletedWorkout);
    }
}
using YogaApp.Application.Mappers.User;
using YogaApp.Application.Mappers.Workout;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Core.Enums;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.User;

public class UserWorkoutService : IUserWorkoutService
{
    private readonly IUserWorkoutRepository _userWorkoutRepository;
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ILogger<UserWorkoutService> _logger;

    public UserWorkoutService(IUserWorkoutRepository userWorkoutRepository, 
        IWorkoutRepository workoutRepository,
        ILogger<UserWorkoutService> logger)
    {
        _workoutRepository = workoutRepository;
        _userWorkoutRepository = userWorkoutRepository;
        _logger = logger;
    }
    public async Task<UserWorkoutDto> StartWorkoutAsync(Guid userId, Guid workoutId)
{
    _logger.LogInformation("User {UserId} is starting workout {WorkoutId}", userId, workoutId);

    var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
    if (workout == null)
    {
        _logger.LogWarning("Workout with ID {WorkoutId} not found", workoutId);
        throw new ArgumentException("Workout not found", nameof(workoutId));
    }

    var userWorkout = new UserWorkoutDto()
    {
        UserWorkoutId = Guid.NewGuid(),
        UserId = userId,
        WorkoutId = workoutId,
        StartedAt = DateTime.UtcNow,
        CompletedAt = null,
        UserName = ",",
        Workout = workout.ToWorkoutDto()
    };

    var savedWorkout = await _userWorkoutRepository.StartWorkoutAsync(userWorkout.ToUserWorkoutModel());

    _logger.LogInformation("Workout {WorkoutId} started successfully for user {UserId}", workoutId, userId);
    
    return savedWorkout.ToUserWorkoutDto();
}

public async Task CompleteWorkoutAsync(Guid userId, Guid workoutId)
{
    _logger.LogInformation("Completing workout {WorkoutId} for user {UserId}", workoutId, userId);

    var userWorkout = await _userWorkoutRepository.GetByUserAndWorkoutAsync(userId, workoutId);

    if (userWorkout == null)
    {
        _logger.LogWarning("Cannot complete workout. Workout not found for user {UserId} and workout {WorkoutId}", userId, workoutId);
        throw new InvalidOperationException("Workout not found.");
    }

    userWorkout.Status = WorkoutStatus.Completed;
    userWorkout.CompletedAt = DateTime.Now;

    await _userWorkoutRepository.UpdateUserWorkoutAsync(userWorkout);

    _logger.LogInformation("Workout {WorkoutId} marked as completed for user {UserId}", workoutId, userId);
}

public async Task UpdateUserWorkoutAsync(UserWorkoutDto updatedWorkout)
{
    _logger.LogInformation("Updating workout {WorkoutId} for user {UserId}", updatedWorkout.WorkoutId, updatedWorkout.UserId);

    var updateWorkout = updatedWorkout.ToUserWorkoutModel();
    await _userWorkoutRepository.UpdateUserWorkoutAsync(updateWorkout);

    _logger.LogInformation("Workout {WorkoutId} updated successfully", updatedWorkout.WorkoutId);
}

public async Task<List<UserWorkoutDto>> GetCompletedWorkouts(Guid? userId)
{
    _logger.LogInformation("Retrieving completed workouts for user {UserId}", userId);

    var completedWorkouts = await _userWorkoutRepository.GetCompletedWorkouts(userId);

    var result = completedWorkouts
        .Where(uw => uw.Workout != null)
        .Select(uw => new UserWorkoutDto
        {
            UserWorkoutId = uw.UserWorkoutId,
            UserId = uw.UserId,
            CompletedAt = uw.CompletedAt,
            Duration = uw.Duration,
            CaloriesBurned = uw.CaloriesBurned,
            Workout = uw.Workout.ToWorkoutDto()
        })
        .ToList();

    _logger.LogInformation("Retrieved {Count} completed workouts for user {UserId}", result.Count, userId);

    return result;
}

}
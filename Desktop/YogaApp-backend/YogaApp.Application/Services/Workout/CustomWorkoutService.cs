using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.Mappers.Workout;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Workout;

public class CustomWorkoutService : ICustomWorkoutService
{
    private readonly ICustomWorkoutRepository _customWorkoutRepository;
    private readonly IUserWorkoutRepository _userWorkoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly ILogger<CustomWorkoutService> _logger;

    public CustomWorkoutService(ICustomWorkoutRepository customWorkoutRepository,
        IUserWorkoutRepository userWorkoutRepository,
        IExerciseRepository exerciseRepository,
        ILogger<CustomWorkoutService> logger)
    {
        _customWorkoutRepository = customWorkoutRepository;
        _userWorkoutRepository = userWorkoutRepository;
        _exerciseRepository = exerciseRepository;
        _logger = logger;
    }

    public async Task<CustomWorkoutResponseDto> CreateWorkoutAsync(CustomWorkoutRequestDto workout)
{
    if (workout == null)
    {
        _logger.LogWarning("Workout creation failed: input is null.");
        throw new ArgumentNullException(nameof(workout), "Workout cannot be null.");
    }

    if (string.IsNullOrWhiteSpace(workout.Name))
    {
        _logger.LogWarning("Workout creation failed: name is empty.");
        throw new ArgumentException("Workout name cannot be empty.", nameof(workout));
    }

    _logger.LogInformation("Creating new workout: {WorkoutName}", workout.Name);
    var createdWorkout = await _customWorkoutRepository.AddCustomWorkoutAsync(workout.ToCustomWorkoutModel());
    _logger.LogInformation("Workout created with ID: {WorkoutId}", createdWorkout.CustomWorkoutId);

    return createdWorkout.ToCustomWorkoutResponseDto();
}

    public async Task<List<CustomWorkoutPreviewDto>> GetAllCustomWorkoutPreviewsAsync()
    {
        _logger.LogInformation("Retrieving all custom workout previews.");
        var workouts = await _customWorkoutRepository.GetAllCustomWorkoutsAsync();
        _logger.LogInformation("Retrieved {Count} custom workouts.", workouts.Count);
        return workouts.Select(w => w.ToCustomWorkoutPreviewDto()).ToList();
    }

    public async Task<List<CustomWorkoutPreviewDto>> SearchWorkoutsByNameAsync(string searchQuery)
    {
        _logger.LogInformation("Searching workouts with query: {Query}", searchQuery);
        var workouts = await _customWorkoutRepository.SearchCustomWorkoutByNameAsync(searchQuery);
        _logger.LogInformation("Found {Count} workouts matching query.", workouts.Count);
        return workouts.Select(w => w.ToCustomWorkoutPreviewDto()).ToList();
    }

    public async Task<CustomWorkoutResponseDto?> GetWorkoutByIdAsync(Guid id)
    {
        _logger.LogInformation("Retrieving workout with ID: {WorkoutId}", id);
        var workout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(id);
        if (workout == null)
        {
            _logger.LogWarning("Workout with ID {WorkoutId} not found.", id);
            return null;
        }

        return workout.ToCustomWorkoutResponseDto();
    }

    public async Task<bool> StartWorkoutAsync(Guid? userId, Guid workoutId)
    {
        _logger.LogInformation("User {UserId} attempting to start workout {WorkoutId}", userId, workoutId);

        var workout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(workoutId);
        if (workout == null || userId == null)
        {
            _logger.LogWarning("Workout start failed. Workout not found or user ID null.");
            return false;
        }

        var existing = await _userWorkoutRepository.GetByUserAndWorkoutAsync(userId, workoutId);
        if (existing != null)
        {
            _logger.LogWarning("Workout already started for user {UserId}", userId);
            return false;
        }

        var userWorkout = new UserWorkoutModel
        {
            UserWorkoutId = Guid.NewGuid(),
            UserId = userId,
            Status = WorkoutStatus.InProgress,
            CompletedAt = null,
            WorkoutId = workoutId,
            StartedAt = DateTime.UtcNow,
        };

        await _userWorkoutRepository.AddAsync(userWorkout);
        _logger.LogInformation("Workout {WorkoutId} started for user {UserId}", workoutId, userId);
        return true;
    }

    public async Task<bool> FinishWorkoutAsync(Guid? userId, Guid workoutId)
    {
        _logger.LogInformation("User {UserId} attempting to finish workout {WorkoutId}", userId, workoutId);

        var workout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(workoutId);
        if (workout == null || userId == null)
        {
            _logger.LogWarning("Workout finish failed. Workout not found or user ID null.");
            return false;
        }

        var existing = await _userWorkoutRepository.GetByUserAndWorkoutAsync(userId, workoutId);
        if (existing == null || existing.CompletedAt != null || existing.StartedAt == null)
        {
            _logger.LogWarning("Workout finish failed. Workout not started or already completed.");
            return false;
        }

        if (workout.CaloriesBurned == null)
        {
            _logger.LogWarning("Workout finish failed. CaloriesBurned is null.");
            return false;
        }

        existing.CompletedAt = DateTime.UtcNow;
        existing.Status = WorkoutStatus.Completed;
        existing.Duration = (int)(existing.CompletedAt.Value - existing.StartedAt.Value).TotalMinutes;
        existing.CaloriesBurned = existing.Duration * workout.CaloriesBurned.Value / 60;

        if (existing.User != null && !string.IsNullOrWhiteSpace(existing.UserName))
        {
            existing.User.Name = existing.UserName;
        }

        await _userWorkoutRepository.UpdateUserWorkoutAsync(existing);
        _logger.LogInformation("Workout {WorkoutId} successfully finished for user {UserId}", workoutId, userId);
        return true;
    }

    public async Task<bool> AddExerciseToWorkoutAsync(Guid workoutId, ExerciseRequestDto exerciseDto, string? existingExerciseName)
    {
        _logger.LogInformation("Adding exercise to workout {WorkoutId}", workoutId);

        var workout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(workoutId);
        if (workout == null)
        {
            _logger.LogWarning("Workout not found: {WorkoutId}", workoutId);
            throw new KeyNotFoundException("Workout not found");
        }

        ExerciseModel? exercise;

        if (!string.IsNullOrEmpty(existingExerciseName))
        {
            _logger.LogInformation("Looking for existing exercise by name: {ExerciseName}", existingExerciseName);
            exercise = await _exerciseRepository.GetExerciseByNameAsync(existingExerciseName);
            if (exercise == null)
            {
                _logger.LogWarning("Exercise not found: {ExerciseName}", existingExerciseName);
                throw new ArgumentException("Exercise not found with that name");
            }
        }
        else
        {
            _logger.LogInformation("Creating new exercise: {ExerciseName}", exerciseDto.Name);
            exercise = new ExerciseModel
            {
                ExerciseId = Guid.NewGuid(),
                Name = exerciseDto.Name,
                Description = exerciseDto.Description,
                Duration = exerciseDto.Duration
            };
            await _exerciseRepository.AddExerciseAsync(exercise);
        }

        if (workout.Exercises != null && workout.Exercises.Any(e => e.ExerciseId == exercise.ExerciseId))
        {
            _logger.LogWarning("Exercise already exists in workout {WorkoutId}", workoutId);
            throw new InvalidOperationException("Exercise already exists in the workout.");
        }

        workout.Exercises?.Add(exercise);
        await _customWorkoutRepository.UpdateCustomWorkoutAsync(workout);
        _logger.LogInformation("Exercise {ExerciseId} added to workout {WorkoutId}", exercise.ExerciseId, workoutId);
        return true;
    }

    public async Task<CustomWorkoutResponseDto> UpdateWorkoutAsync(Guid id, CustomWorkoutRequestDto? workoutDto)
    {
        _logger.LogInformation("Updating workout {WorkoutId}", id);

        var existingWorkout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(id);
        if (existingWorkout == null)
        {
            _logger.LogWarning("Workout not found for update: {WorkoutId}", id);
            throw new KeyNotFoundException("Custom workout not found");
        }

        if (workoutDto?.Name != null) existingWorkout.Name = workoutDto.Name;
        if (workoutDto?.ImageUrl != null || workoutDto?.ImageUrl == null) existingWorkout.ImageUrl = workoutDto?.ImageUrl;
        if (workoutDto?.Difficulty != null) existingWorkout.Difficulty = workoutDto.Difficulty;
        if (workoutDto?.Duration != null) existingWorkout.Duration = workoutDto.Duration.Value;
        if (workoutDto?.CaloriesBurned != null) existingWorkout.CaloriesBurned = workoutDto.CaloriesBurned.Value;
        if (workoutDto?.Category != null) existingWorkout.Category = workoutDto.Category;
        if (workoutDto?.Exercises != null)
            existingWorkout.Exercises = workoutDto.Exercises.Select(e => e.ToExerciseModel()).ToList();

        await _customWorkoutRepository.UpdateCustomWorkoutAsync(existingWorkout);
        _logger.LogInformation("Workout {WorkoutId} updated successfully", id);
        return existingWorkout.ToCustomWorkoutResponseDto();
    }

    public async Task<bool> DeleteWorkoutAsync(Guid id)
    {
        _logger.LogInformation("Deleting workout {WorkoutId}", id);
        var existingWorkout = await _customWorkoutRepository.GetCustomWorkoutByIdAsync(id);
        if (existingWorkout == null)
        {
            _logger.LogWarning("Workout not found for deletion: {WorkoutId}", id);
            return false;
        }

        await _customWorkoutRepository.DeleteCustomWorkoutAsync(id);
        _logger.LogInformation("Workout {WorkoutId} deleted successfully", id);
        return true;
    }

}
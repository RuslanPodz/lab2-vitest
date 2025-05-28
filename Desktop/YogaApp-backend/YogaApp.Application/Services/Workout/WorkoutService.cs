using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.Mappers.Workout;
using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.Progress;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Workout;

public class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUserWorkoutRepository _userWorkoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUserGeneralStatsService _userGeneralStatsService;
    private readonly ILogger<WorkoutService> _logger;
    
    public WorkoutService(IWorkoutRepository workoutRepository, 
        IExerciseRepository exerciseRepository, 
        IUserWorkoutRepository userWorkoutRepository,
        IUserGeneralStatsService userGeneralStatsService,
        ILogger<WorkoutService> logger)
    {
        _workoutRepository = workoutRepository;
        _userWorkoutRepository = userWorkoutRepository;
        _exerciseRepository = exerciseRepository;
        _userGeneralStatsService = userGeneralStatsService;
        _logger = logger;
    }
     public async Task<List<WorkoutResponseDto>> GetAllAsync()
    {
        var workouts = await _workoutRepository.GetAllWorkoutsAsync();
        _logger.LogInformation("Retrieved all workouts. Count: {Count}", workouts.Count);

        return workouts.Select(w => new WorkoutResponseDto
        {
            WorkoutId = w.WorkoutId,
            Name = w.Name,
            Difficulty = w.Difficulty,
            Duration = w.Duration,
            CaloriesBurned = w.CaloriesBurned
        }).ToList();
    }

    public async Task<WorkoutResponseDto?> GetWorkoutByIdAsync(Guid id)
    {
        var workout = await _workoutRepository.GetWorkoutByIdAsync(id);

        if (workout == null)
        {
            _logger.LogWarning("Workout not found with ID: {WorkoutId}", id);
            return null;
        }

        _logger.LogInformation("Workout retrieved with ID: {WorkoutId}", id);
        return new WorkoutResponseDto
        {
            WorkoutId = workout.WorkoutId,
            Name = workout.Name,
            Difficulty = workout.Difficulty,
            Duration = workout.Duration,
            CaloriesBurned = workout.CaloriesBurned
        };
    }

    public async Task<List<WorkoutPreviewDto>> SearchWorkoutsByNameAsync(string searchQuery)
    {
        var workouts = await _workoutRepository.SearchWorkouts(searchQuery);
        _logger.LogInformation("Searched workouts by query '{Query}'. Results: {Count}", searchQuery, workouts.Count);
        return workouts.Select(w => w.ToWorkoutPreviewDto()).ToList();
    }

    public async Task<List<WorkoutPreviewDto>> GetWorkoutsByCategoryPreviewAsync(string categoryName)
    {
        var workouts = await _workoutRepository.GetWorkoutsByCategoryAsync(categoryName);
        _logger.LogInformation("Retrieved preview for workouts in category: {Category}", categoryName);
        return workouts.Select(w => w.ToWorkoutPreviewDto()).ToList();
    }

    public async Task<List<WorkoutResponseDto>?> GetWorkoutsByCategoryAsync(string categoryName)
    {
        var workouts = await _workoutRepository.GetWorkoutsByCategoryAsync(categoryName);
        _logger.LogInformation("Retrieved workouts by category: {Category}. Count: {Count}", categoryName, workouts.Count);

        return workouts.Select(w => new WorkoutResponseDto
        {
            WorkoutId = w.WorkoutId,
            Name = w.Name,
            CaloriesBurned = w.CaloriesBurned,
            Category = w.Category,
            ImageUrl = w.ImageUrl,
            LastPerformedAt = w.LastPerformedAt,
            IsFavourite = w.IsFavourite,
            Duration = w.Duration,
            Difficulty = w.Difficulty,
            Exercises = (w.Exercises ?? throw new InvalidOperationException()).Select(e => e.ToExerciseDto()).ToList()
        }).ToList();
    }

    public async Task<List<WorkoutResponseDto>> GetFavouriteWorkoutsAsync(Guid userId)
    {
        var result = await _userWorkoutRepository.GetFavouriteWorkoutsAsync(userId);
        _logger.LogInformation("Retrieved favourite workouts for user: {UserId}. Count: {Count}", userId, result.Count);
        return result.Select(w => w.ToWorkoutResponseDto()).ToList();
    }

    public async Task<List<WorkoutResponseDto>> GetRecentWorkoutsAsync(Guid? userId)
    {
        var result = await _userWorkoutRepository.GetRecentWorkoutsAsync(userId);
        _logger.LogInformation("Retrieved recent workouts for user: {UserId}. Count: {Count}", userId, result.Count);
        return result.Select(w => w.ToWorkoutResponseDto()).ToList();
    }

    public async Task<bool> StartWorkoutAsync(Guid? userId, Guid workoutId)
    {
        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
        if (workout == null || userId == null)
        {
            _logger.LogWarning("Failed to start workout. Invalid user or workout ID. UserId: {UserId}, WorkoutId: {WorkoutId}", userId, workoutId);
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
        _logger.LogInformation("Workout started. WorkoutId: {WorkoutId}, UserId: {UserId}", workoutId, userId);
        return true;
    }

    public async Task<bool> FinishWorkoutAsync(Guid? userId, Guid workoutId)
    {
        if (userId == null)
        {
            _logger.LogWarning("User ID is null while trying to finish workout {WorkoutId}", workoutId);
            return false;
        }

        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
        if (workout == null)
        {
            _logger.LogWarning("Workout not found when trying to finish: {WorkoutId}", workoutId);
            return false;
        }

        var existing = await _userWorkoutRepository.GetActiveUserWorkoutAsync(userId.Value, workoutId);
        if (existing == null || existing.StartedAt == null)
        {
            _logger.LogWarning("No active workout session found to finish. WorkoutId: {WorkoutId}, UserId: {UserId}", workoutId, userId);
            return false;
        }

        if (workout.CaloriesBurned == null)
        {
            _logger.LogWarning("Workout calories burned is null. Cannot calculate.");
            return false;
        }

        existing.CompletedAt = DateTime.UtcNow;
        existing.LastPerformedAt = DateTime.UtcNow;
        existing.Status = WorkoutStatus.Completed;
        existing.Duration = (int)(existing.CompletedAt.Value - existing.StartedAt.Value).TotalMinutes;
        existing.CaloriesBurned = existing.Duration * workout.CaloriesBurned.Value / 60;
        
        await _userWorkoutRepository.UpdateUserWorkoutAsync(existing);
        _logger.LogInformation("Workout finished. WorkoutId: {WorkoutId}, UserId: {UserId}", workoutId, userId);
        await _userGeneralStatsService.UpdateGeneralStats(userId);
        return true;
    }
    
    public async Task<WorkoutResponseDto> CreateWorkoutAsync(WorkoutRequestDto workout)
    {
        if (workout == null)
            throw new ArgumentNullException(nameof(workout), "Workout data is required");

        _logger.LogInformation("Creating new workout: {WorkoutName}", workout.Name);

        var newWorkout = await _workoutRepository.AddWorkoutAsync(workout.ToWorkoutModel());

        _logger.LogInformation("Workout created with ID: {WorkoutId}", newWorkout.WorkoutId);

        return newWorkout.ToWorkoutResponseDto();
    }

    public async Task<bool> AddExerciseToWorkoutAsync(Guid workoutId, ExerciseRequestDto exerciseDto, string? existingExerciseName)
    {
        _logger.LogInformation("Adding exercise to workout {WorkoutId}. Existing name: {ExerciseName}", workoutId, existingExerciseName);

        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
        if (workout == null)
        {
            _logger.LogWarning("Workout {WorkoutId} not found", workoutId);
            throw new KeyNotFoundException("Workout not found");
        }

        ExerciseModel? exercise;

        if (!string.IsNullOrEmpty(existingExerciseName))
        {
            exercise = await _exerciseRepository.GetExerciseByNameAsync(existingExerciseName);
            if (exercise == null)
            {
                _logger.LogWarning("Exercise with name '{Name}' not found", existingExerciseName);
                throw new ArgumentException("Exercise not found with that name");
            }
        }
        else
        {
            exercise = new ExerciseModel
            {
                ExerciseId = Guid.NewGuid(),
                Name = exerciseDto.Name,
                Description = exerciseDto.Description,
                Duration = exerciseDto.Duration
            };

            await _exerciseRepository.AddExerciseAsync(exercise);
            _logger.LogInformation("New exercise created: {ExerciseId}", exercise.ExerciseId);
        }

        if (workout.Exercises != null && workout.Exercises.Any(e => e.ExerciseId == exercise.ExerciseId))
        {
            _logger.LogWarning("Exercise {ExerciseId} already exists in workout {WorkoutId}", exercise.ExerciseId, workoutId);
            throw new InvalidOperationException("Exercise already exists in the workout.");
        }

        workout.Exercises?.Add(exercise);
        await _workoutRepository.UpdateAsync(workout);

        _logger.LogInformation("Exercise {ExerciseId} added to workout {WorkoutId}", exercise.ExerciseId, workoutId);

        return true;
    }
    
    public async Task<WorkoutResponseDto> UpdateWorkoutAsync(Guid id, WorkoutRequestDto? workoutDto)
    {
        _logger.LogInformation("Updating workout with ID: {WorkoutId}", id);

        var existingWorkout = await _workoutRepository.GetWorkoutByIdAsync(id);
        if (existingWorkout == null)
        {
            _logger.LogWarning("Workout {WorkoutId} not found", id);
            throw new KeyNotFoundException("Workout not found");
        }

        if (workoutDto?.Name != null) existingWorkout.Name = workoutDto.Name;
        if (workoutDto?.ImageUrl != null || workoutDto?.ImageUrl == null) existingWorkout.ImageUrl = workoutDto?.ImageUrl;
        if (workoutDto?.Difficulty != null) existingWorkout.Difficulty = workoutDto.Difficulty;
        if (workoutDto?.Duration != null) existingWorkout.Duration = workoutDto.Duration.Value;
        if (workoutDto?.CaloriesBurned != null) existingWorkout.CaloriesBurned = workoutDto.CaloriesBurned.Value;
        if (workoutDto?.Category != null) existingWorkout.Category = workoutDto.Category;
        if (workoutDto?.Exercises != null) existingWorkout.Exercises = workoutDto.Exercises.Select(e => e.ToExerciseModel()).ToList();

        await _workoutRepository.UpdateAsync(existingWorkout);

        _logger.LogInformation("Workout {WorkoutId} updated successfully", id);

        return existingWorkout.ToWorkoutResponseDto();
    }

    public async Task<bool> DeleteWorkoutAsync(Guid id)
    {
        _logger.LogInformation("Attempting to delete workout {WorkoutId}", id);

        var existingWorkout = await _workoutRepository.GetWorkoutByIdAsync(id);
        if (existingWorkout == null)
        {
            _logger.LogWarning("Workout {WorkoutId} not found", id);
            return false;
        }

        await _workoutRepository.DeleteAsync(existingWorkout.WorkoutId);
        _logger.LogInformation("Workout {WorkoutId} deleted", id);

        return true;
    }
    
    public async Task<bool> AddWorkoutToFavouritesAsync(Guid userId, Guid workoutId)
    {
        _logger.LogInformation("Adding workout {WorkoutId} to favourites for user {UserId}", workoutId, userId);

        var workout = await _workoutRepository.GetWorkoutByIdAsync(workoutId);
        if (workout == null)
        {
            _logger.LogWarning("Workout {WorkoutId} not found", workoutId);
            throw new KeyNotFoundException("Workout not found");
        }

        var existing = await _userWorkoutRepository.GetByUserAndWorkoutAsync(userId, workoutId);
        if (existing != null)
        {
            if (existing.IsFavourite)
            {
                _logger.LogInformation("Workout {WorkoutId} is already in favourites for user {UserId}", workoutId, userId);
                return false;
            }

            existing.IsFavourite = true;
            await _userWorkoutRepository.UpdateUserWorkoutAsync(existing);
            _logger.LogInformation("Workout {WorkoutId} marked as favourite for user {UserId}", workoutId, userId);
        }
        else
        {
            var newUserWorkout = new UserWorkoutModel()
            {
                UserId = userId,
                WorkoutId = workoutId,
                IsFavourite = true
            };

            await _userWorkoutRepository.AddAsync(newUserWorkout);
            _logger.LogInformation("Workout {WorkoutId} added to favourites for user {UserId}", workoutId, userId);
        }

        return true;
    }
}

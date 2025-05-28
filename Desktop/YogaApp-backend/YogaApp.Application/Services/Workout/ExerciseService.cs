using YogaApp.Application.Mappers.Workout;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Workout;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ILogger<ExerciseService> _logger;
    
    public ExerciseService(IExerciseRepository exerciseRepository, 
        IWorkoutRepository workoutRepository,
        ILogger<ExerciseService> logger)
    {
        _exerciseRepository = exerciseRepository;
        _workoutRepository = workoutRepository;
        _logger = logger;
    }
    public async Task<ExerciseResponseDto> GetExerciseByNameAsync(ExerciseRequestDto request)
    {
        _logger.LogInformation("Getting exercise by name: {ExerciseName}", request.Name);

        var exercise = await _exerciseRepository.GetExerciseByNameAsync(request.Name);

        if (exercise == null)
        {
            _logger.LogWarning("Exercise with name '{ExerciseName}' not found.", request.Name);
            throw new Exception($"Exercise with name '{request.Name}' not found.");
        }

        _logger.LogInformation("Exercise found: {ExerciseId}", exercise.ExerciseId);
        return exercise.ToExerciseResponseDto();
    }

    public async Task<ExerciseResponseDto> AddExerciseAsync(ExerciseRequestDto request)
    {
        _logger.LogInformation("Adding new exercise: {ExerciseName}", request.Name);

        var model = request.ToExerciseModel();
        await _exerciseRepository.AddExerciseAsync(model);

        _logger.LogInformation("Exercise added with ID: {ExerciseId}", model.ExerciseId);
        return model.ToExerciseResponseDto();
    }

    public async Task DeleteExerciseAsync(Guid id)
    {
        _logger.LogInformation("Deleting exercise with ID: {ExerciseId}", id);

        await _exerciseRepository.DeleteExerciseAsync(id);

        _logger.LogInformation("Exercise with ID: {ExerciseId} deleted", id);
    }
}
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

namespace YogaApp.Application.ServiceContact.Workout;

public interface IBaseWorkoutService<TPreviewDto, TResponseDto, TRequestDto>
{
    Task<List<TPreviewDto>> SearchWorkoutsByNameAsync(string searchQuery);
    Task<TResponseDto?> GetWorkoutByIdAsync(Guid id);
    Task<TResponseDto> CreateWorkoutAsync(TRequestDto workoutDto);
    Task<TResponseDto> UpdateWorkoutAsync(Guid id, TRequestDto workoutDto);
    Task<bool> DeleteWorkoutAsync(Guid id);
    Task<bool> StartWorkoutAsync(Guid? userId, Guid workoutId);
    Task<bool> FinishWorkoutAsync(Guid? userId, Guid workoutId);
    Task<bool> AddExerciseToWorkoutAsync(Guid workoutId, ExerciseRequestDto exerciseDto, string? existingExerciseName);
}
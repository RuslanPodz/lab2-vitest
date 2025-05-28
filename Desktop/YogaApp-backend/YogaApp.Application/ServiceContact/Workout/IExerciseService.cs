using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

namespace YogaApp.Application.ServiceContact.Workout;

public interface IExerciseService
{
    Task<ExerciseResponseDto> GetExerciseByNameAsync(ExerciseRequestDto request);
    Task<ExerciseResponseDto> AddExerciseAsync(ExerciseRequestDto request);
    Task DeleteExerciseAsync(Guid id);
}
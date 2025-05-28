using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

namespace YogaApp.Application.ServiceContact.Workout;

public interface IWorkoutService : IBaseWorkoutService<WorkoutPreviewDto, WorkoutResponseDto, WorkoutRequestDto>
{
    Task<List<WorkoutPreviewDto>> GetWorkoutsByCategoryPreviewAsync(string categoryName);
    Task<List<WorkoutResponseDto>> GetFavouriteWorkoutsAsync(Guid userId);
    Task<List<WorkoutResponseDto>> GetRecentWorkoutsAsync(Guid? userId);
    Task<bool> AddWorkoutToFavouritesAsync(Guid userId, Guid workoutId);
}
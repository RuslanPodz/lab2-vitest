using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace YogaApp.Application.ServiceContact.User;

public interface IUserWorkoutService
{
    Task<UserWorkoutDto> StartWorkoutAsync(Guid userId, Guid workoutId);
    Task CompleteWorkoutAsync(Guid userId, Guid workoutId);
    Task UpdateUserWorkoutAsync(UserWorkoutDto updatedWorkout);
    Task<List<UserWorkoutDto>> GetCompletedWorkouts(Guid? userId);
}
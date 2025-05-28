using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Core.RepositoryContracts;

public interface IUserWorkoutRepository
{
    Task<UserWorkoutModel?> GetByUserAndWorkoutAsync(Guid? userId, Guid workoutId);
    Task<UserWorkoutModel> StartWorkoutAsync(UserWorkoutModel userWorkout);
    Task<List<WorkoutModel?>> GetFavouriteWorkoutsAsync(Guid userId);
    Task UpdateUserWorkoutAsync(UserWorkoutModel userWorkout);
    Task<UserWorkoutModel?> GetActiveUserWorkoutAsync(Guid userId, Guid workoutId);
    Task<List<WorkoutModel>> GetRecentWorkoutsAsync(Guid? userId);
    Task AddAsync(UserWorkoutModel userWorkout);
    Task<List<UserWorkoutModel>> GetCompletedWorkouts(Guid? userId);
}
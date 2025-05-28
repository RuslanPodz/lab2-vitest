using YogaApp.Core.Domain.Workout;

namespace YogaApp.Core.RepositoryContracts;

public interface ICustomWorkoutRepository
{
    Task<List<CustomWorkoutModel>> GetAllCustomWorkouts();
    Task<CustomWorkoutModel> AddCustomWorkoutAsync(CustomWorkoutModel workout);
    Task<List<CustomWorkoutModel>> SearchCustomWorkoutByNameAsync(string name);
    Task<CustomWorkoutModel?> GetCustomWorkoutByIdAsync(Guid workoutId);
    Task<List<CustomWorkoutModel>> GetAllCustomWorkoutsAsync();
    Task<CustomWorkoutModel?> GetCustomWorkoutByNameAsync(string name);
    Task<CustomWorkoutModel?> UpdateCustomWorkoutAsync(CustomWorkoutModel workout);
    
    Task DeleteCustomWorkoutAsync(Guid workoutId);
}
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;

namespace YogaApp.Core.RepositoryContracts;

public interface IWorkoutRepository
{
    Task<List<WorkoutModel>> GetAllWorkoutsAsync();
    
    Task<WorkoutModel> GetWorkoutByNameAsync(string? name);
    Task<List<WorkoutModel>> SearchCustomWorkoutByNameAsync(string name);
    Task<WorkoutModel?> GetWorkoutByIdAsync(Guid workoutId);

    Task<List<WorkoutModel>> GetWorkoutsByCategoryAsync(string categoryName);
    
    Task<List<WorkoutModel>> SearchWorkouts(string searchText);
    
    Task<WorkoutModel> AddWorkoutAsync(WorkoutModel workout);

    Task UpdateAsync(WorkoutModel workout);

    Task DeleteAsync(Guid id);
}
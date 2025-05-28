using YogaApp.Core.Domain.Workout;

namespace YogaApp.Core.RepositoryContracts;

public interface IExerciseRepository
{
    Task<ExerciseModel> AddExerciseAsync(ExerciseModel exercise);
    Task DeleteExerciseAsync(Guid exerciseId);
    Task<ExerciseModel?> GetExerciseByNameAsync(string name);
}
using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Workout;

public class ExerciseRepository : IExerciseRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ExerciseRepository> _logger;

    public ExerciseRepository(AppDbContext context, ILogger<ExerciseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ExerciseModel> AddExerciseAsync(ExerciseModel exercise)
    {
        if (exercise == null)
        {
            _logger.LogWarning("Attempted to add a null exercise.");
            throw new ArgumentNullException(nameof(exercise), "Exercise cannot be null.");
        }

        var newExercise = await _context.Exercises.AddAsync(exercise);
        await _context.SaveChangesAsync();

        if (newExercise.Entity == null)
        {
            _logger.LogError("Failed to add exercise to the database.");
            throw new InvalidOperationException("Failed to add exercise to the database.");
        }

        _logger.LogInformation("Exercise added successfully: {ExerciseId}", newExercise.Entity.ExerciseId);
        return newExercise.Entity;
    }

    public async Task DeleteExerciseAsync(Guid exerciseId)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e != null && e.ExerciseId == exerciseId);

        if (exercise == null)
        {
            _logger.LogWarning("Attempted to delete non-existing exercise with ID: {ExerciseId}", exerciseId);
            return;
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Exercise deleted with ID: {ExerciseId}", exerciseId);
    }

    public async Task<ExerciseModel?> GetExerciseByNameAsync(string name)
    {
        _logger.LogInformation("Searching for exercise by name: {Name}", name);

        var exercise = await _context.Exercises
            .FirstOrDefaultAsync(e => e != null && e.Name.ToLower() == name.ToLower());

        if (exercise == null)
            _logger.LogWarning("No exercise found with name: {Name}", name);

        return exercise;
    }
}

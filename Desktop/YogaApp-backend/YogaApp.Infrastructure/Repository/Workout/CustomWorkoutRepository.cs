using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Workout;

public class CustomWorkoutRepository : ICustomWorkoutRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomWorkoutRepository> _logger;

    public CustomWorkoutRepository(AppDbContext context, ILogger<CustomWorkoutRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<CustomWorkoutModel>> GetAllCustomWorkouts()
    {
        _logger.LogInformation("Fetching all custom workouts.");
        var customWorkouts = await _context.CustomWorkouts.ToListAsync();
        _logger.LogInformation("{Count} custom workouts retrieved.", customWorkouts.Count);
        return customWorkouts;
    }

    public async Task<CustomWorkoutModel> AddCustomWorkoutAsync(CustomWorkoutModel workout)
    {
        workout.CustomWorkoutId = Guid.NewGuid();
        _logger.LogInformation("Adding new custom workout: {Name}", workout.Name);

        await _context.CustomWorkouts.AddAsync(workout);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Custom workout added with ID: {Id}", workout.CustomWorkoutId);
        return workout;
    }

    public async Task<List<CustomWorkoutModel>> SearchCustomWorkoutByNameAsync(string searchQuery)
    {
        _logger.LogInformation("Searching custom workouts with query: {Query}", searchQuery);

        var results = await _context.CustomWorkouts
            .Where(w => w.Category != null &&
                        w.Name != null &&
                        (EF.Functions.ILike(w.Name, $"%{searchQuery}%") ||
                         EF.Functions.ILike(w.Category, $"%{searchQuery}%")))
            .ToListAsync();

        _logger.LogInformation("Found {Count} custom workouts matching query.", results.Count);
        return results;
    }

    public async Task<CustomWorkoutModel?> GetCustomWorkoutByIdAsync(Guid workoutId)
    {
        _logger.LogInformation("Getting custom workout by ID: {Id}", workoutId);

        var customWorkout = await _context.CustomWorkouts
            .Include(cw => cw.Exercises)
            .FirstOrDefaultAsync(cw => cw.CustomWorkoutId == workoutId);

        if (customWorkout == null)
            _logger.LogWarning("No custom workout found with ID: {Id}", workoutId);

        return customWorkout;
    }

    public async Task<List<CustomWorkoutModel>> GetAllCustomWorkoutsAsync()
    {
        _logger.LogInformation("Fetching all custom workouts (async method).");
        var customWorkouts = await _context.CustomWorkouts.ToListAsync();
        _logger.LogInformation("{Count} custom workouts retrieved.", customWorkouts.Count);
        return customWorkouts;
    }

    public async Task<CustomWorkoutModel?> GetCustomWorkoutByNameAsync(string name)
    {
        _logger.LogInformation("Getting custom workout by name: {Name}", name);

        var workout = await _context.CustomWorkouts
            .FirstOrDefaultAsync(c => c.Name != null && c.Name.ToLower() == name.ToLower());

        if (workout == null)
            _logger.LogWarning("No custom workout found with name: {Name}", name);

        return workout;
    }

    public async Task<CustomWorkoutModel?> UpdateCustomWorkoutAsync(CustomWorkoutModel workout)
    {
        _logger.LogInformation("Updating custom workout with ID: {Id}", workout.CustomWorkoutId);

        var existingWorkout = await _context.CustomWorkouts.FindAsync(workout.CustomWorkoutId);
        if (existingWorkout == null)
        {
            _logger.LogWarning("Workout with ID {Id} not found for update.", workout.CustomWorkoutId);
            return null;
        }

        _context.Entry(existingWorkout).CurrentValues.SetValues(workout);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Workout with ID {Id} updated successfully.", workout.CustomWorkoutId);
        return existingWorkout;
    }

    public async Task DeleteCustomWorkoutAsync(Guid workoutId)
    {
        _logger.LogInformation("Deleting custom workout with ID: {Id}", workoutId);

        var deletedWorkout = await _context.CustomWorkouts.FindAsync(workoutId);
        if (deletedWorkout != null)
        {
            _context.CustomWorkouts.Remove(deletedWorkout);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Workout with ID {Id} deleted successfully.", workoutId);
        }
        else
        {
            _logger.LogWarning("No workout found with ID: {Id} to delete.", workoutId);
        }
    }
}

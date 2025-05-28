using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Workout;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<WorkoutRepository> _logger;

    public WorkoutRepository(AppDbContext context, ILogger<WorkoutRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<List<WorkoutModel>> GetAllWorkoutsAsync()
    {
        _logger.LogInformation("Fetching all workouts");
        return await _context.Workouts
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<WorkoutModel> GetWorkoutByNameAsync(string? name)
    {
        _logger.LogInformation("Searching workout by name: {Name}", name);

        var workout = await _context.Workouts.FirstOrDefaultAsync(w => name != null && w.Name != null && w.Name.ToLower() == name.ToLower());

        if (workout == null)
        {
            _logger.LogWarning("Workout with name '{Name}' not found", name);
            throw new KeyNotFoundException($"Workout with name '{name}' not found.");
        }

        _logger.LogInformation("Workout found: {WorkoutId}", workout.WorkoutId);
        return workout;
    }


    public async Task<List<WorkoutModel>> SearchCustomWorkoutByNameAsync(string name)
    {
        _logger.LogInformation("Searching workouts containing: {Name}", name);
        return await _context.Workouts
            .Where(w => w.Name != null && w.Name.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }


    public async Task<WorkoutModel?> GetWorkoutByIdAsync(Guid workoutId)
    {
        _logger.LogInformation("Fetching workout by ID: {WorkoutId}", workoutId);
        return await _context.Workouts.FirstOrDefaultAsync(w => w.WorkoutId == workoutId);
    }


    public async Task<List<WorkoutModel>> GetWorkoutsByCategoryAsync(string categoryName)
    {
        
        if (string.IsNullOrEmpty(categoryName))
        {
            _logger.LogInformation("No category specified. Returning all workouts.");
            return await _context.Workouts
                .AsNoTracking()
                .ToListAsync();
        }
        
        _logger.LogInformation("Fetching workouts by category: {Category}", categoryName);
       
        return await _context.Workouts
            .Where(w => w.Category != null && w.Category.ToLower() == categoryName.ToLower())
            .ToListAsync();
    }


    public async Task<List<WorkoutModel>> SearchWorkouts(string searchText)
    {
        _logger.LogInformation("Searching workouts with text: {SearchText}", searchText);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            _logger.LogInformation("Search text is empty. Returning all workouts.");
            return await _context.Workouts.ToListAsync();
        }

        return await _context.Workouts
            .Where(w => w.Category != null &&
                        w.Name != null &&
                        (EF.Functions.ILike(w.Name, $"%{searchText}%") ||
                         EF.Functions.ILike(w.Category, $"%{searchText}%")))
            .ToListAsync();
    }


    public async Task<WorkoutModel> AddWorkoutAsync(WorkoutModel workout)
    {
        _logger.LogInformation("Adding new workout: {WorkoutName}", workout.Name);

        await _context.Workouts.AddAsync(workout);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Workout created with ID: {WorkoutId}", workout.WorkoutId);
        return workout;
    }


    public async Task UpdateAsync(WorkoutModel workout)
    {
        _logger.LogInformation("Updating workout: {WorkoutId}", workout.WorkoutId);

        var existingWorkout = await _context.Workouts.FindAsync(workout.WorkoutId);
        if (existingWorkout != null)
        {
            _context.Entry(existingWorkout).CurrentValues.SetValues(workout);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Workout updated: {WorkoutId}", workout.WorkoutId);
        }
        else
        {
            _logger.LogWarning("Workout not found for update: {WorkoutId}", workout.WorkoutId);
        }
    }


   public async Task DeleteAsync(Guid id)
{
    _logger.LogInformation("Deleting workout: {WorkoutId}", id);

    var workout = await _context.Workouts
        .Include(w => w.Exercises)
        .FirstOrDefaultAsync(w => w.WorkoutId == id);

    if (workout == null)
    {
        _logger.LogWarning("Workout not found: {WorkoutId}", id);
        return;
    }

    workout.Exercises?.Clear();
    _context.Workouts.Remove(workout);
    await _context.SaveChangesAsync();

    _logger.LogInformation("Workout deleted: {WorkoutId}", id);
}

}
using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.User;

public class UserWorkoutRepository : IUserWorkoutRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserWorkoutRepository> _logger;

    public UserWorkoutRepository(AppDbContext context, ILogger<UserWorkoutRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserWorkoutModel?> GetByUserAndWorkoutAsync(Guid? userId, Guid workoutId)
    {
        _logger.LogInformation("Fetching workout for user {UserId} and workout {WorkoutId}", userId, workoutId);
        return await _context.UserWorkouts
            .FirstOrDefaultAsync(uw => uw.UserId == userId && uw.WorkoutId == workoutId);
    }

    public async Task<UserWorkoutModel> StartWorkoutAsync(UserWorkoutModel userWorkout)
    {
        userWorkout.StartedAt = DateTime.UtcNow;
        _logger.LogInformation("Starting new workout for user {UserId} with workout {WorkoutId}", userWorkout.UserId, userWorkout.WorkoutId);

        await _context.UserWorkouts.AddAsync(userWorkout);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Workout started with ID: {UserWorkoutId}", userWorkout.UserWorkoutId);
        return userWorkout;
    }

    public async Task<List<WorkoutModel?>> GetFavouriteWorkoutsAsync(Guid userId)
    {
        _logger.LogInformation("Getting favourite workouts for user {UserId}", userId);

        var favourites = await _context.UserWorkouts
            .Where(uw => uw.UserId == userId && uw.IsFavourite)
            .Include(uw => uw.Workout)
            .Select(uw => uw.Workout)
            .ToListAsync();

        _logger.LogInformation("{Count} favourite workouts found for user {UserId}", favourites.Count, userId);
        return favourites;
    }

    public async Task UpdateUserWorkoutAsync(UserWorkoutModel updatedWorkout)
    {
        _logger.LogInformation("Updating workout with ID: {UserWorkoutId}", updatedWorkout.UserWorkoutId);

        var existing = await _context.UserWorkouts
            .FirstOrDefaultAsync(uw => uw.UserWorkoutId == updatedWorkout.UserWorkoutId);

        if (existing != null)
        {
            _context.Entry(existing).CurrentValues.SetValues(updatedWorkout);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Workout {UserWorkoutId} updated successfully", updatedWorkout.UserWorkoutId);
        }
        else
        {
            _logger.LogWarning("Workout {UserWorkoutId} not found for update", updatedWorkout.UserWorkoutId);
        }
    }

    public async Task<UserWorkoutModel?> GetActiveUserWorkoutAsync(Guid userId, Guid workoutId)
    {
        _logger.LogInformation("Getting active workout for user {UserId} and workout {WorkoutId}", userId, workoutId);

        return await _context.UserWorkouts
            .Where(uw => uw.UserId == userId && uw.WorkoutId == workoutId && uw.CompletedAt == null)
            .OrderByDescending(uw => uw.StartedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<List<WorkoutModel>> GetRecentWorkoutsAsync(Guid? userId)
    {
        if (userId == null)
        {
            _logger.LogWarning("Null userId provided to GetRecentWorkoutsAsync");
            return new List<WorkoutModel>();
        }

        _logger.LogInformation("Getting recent workouts for user {UserId}", userId);

        var lastPerformedDates = await _context.UserWorkouts
            .Where(uw => uw.UserId == userId && uw.CompletedAt != null)
            .GroupBy(uw => uw.WorkoutId)
            .Select(g => new
            {
                WorkoutId = g.Key,
                LastPerformedAt = g.Max(x => x.CompletedAt)
            })
            .ToListAsync();

        var workouts = await _context.Workouts
            .Where(w => lastPerformedDates.Select(ld => ld.WorkoutId).Contains(w.WorkoutId))
            .ToListAsync();

        foreach (var workout in workouts)
        {
            workout.LastPerformedAt = lastPerformedDates
                .FirstOrDefault(ld => ld.WorkoutId == workout.WorkoutId)?.LastPerformedAt;
        }

        var recentWorkouts = workouts
            .Where(w => w.LastPerformedAt != null)
            .OrderByDescending(w => w.LastPerformedAt)
            .Take(10)
            .ToList();

        _logger.LogInformation("{Count} recent workouts found for user {UserId}", recentWorkouts.Count, userId);
        return recentWorkouts;
    }

    public async Task AddAsync(UserWorkoutModel userWorkout)
    {
        _logger.LogInformation("Adding new UserWorkout for user {UserId}", userWorkout.UserId);
        await _context.UserWorkouts.AddAsync(userWorkout);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserWorkoutModel>> GetCompletedWorkouts(Guid? userId)
    {
        _logger.LogInformation("Getting completed workouts for user {UserId}", userId);

        var completedWorkouts = await _context.UserWorkouts
            .Include(uw => uw.Workout)
            .Where(uw => uw.UserId == userId && uw.Status == WorkoutStatus.Completed)
            .ToListAsync();

        _logger.LogInformation("{Count} completed workouts found for user {UserId}", completedWorkouts.Count, userId);
        return completedWorkouts;
    }
}

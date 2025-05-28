using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Progress;

public class UserGeneralStatsRepository : IUserGeneralStatsRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserGeneralStatsRepository> _logger;

    public UserGeneralStatsRepository(AppDbContext context, ILogger<UserGeneralStatsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserGeneralStatsModel?> GetByUserIdAsync(Guid? userId)
    {
        _logger.LogInformation("Retrieving general stats for user ID: {UserId}", userId);
        var stats = await _context.UserGeneralStats.FirstOrDefaultAsync(u => u.UserId == userId);
        if (stats == null)
            _logger.LogWarning("No general stats found for user ID: {UserId}", userId);
        return stats;
    }

    public async Task UpdateAsync(UserGeneralStatsModel stats)
    {
        _logger.LogInformation("Updating general stats for user ID: {UserId}", stats.UserId);
        var existingStats = await _context.UserGeneralStats.FindAsync(stats.UserId);

        if (existingStats != null)
        {
            _logger.LogInformation("Stats found for user ID: {UserId}, updating...", stats.UserId);
            _context.Entry(existingStats).CurrentValues.SetValues(stats);
        }
        else
        {
            _logger.LogInformation("No existing stats for user ID: {UserId}, creating new.", stats.UserId);
            await _context.UserGeneralStats.AddAsync(stats);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("General stats saved for user ID: {UserId}", stats.UserId);
    }

    public async Task IncrementWorkoutCountAsync(Guid userId, int duration, int calories)
    {
        _logger.LogInformation("Incrementing workout stats for user ID: {UserId}", userId);
        var stats = await GetByUserIdAsync(userId);

        if (stats != null)
        {
            stats.TotalWorkouts++;
            stats.TotalMinutes += duration;
            stats.TotalCaloriesBurned += calories;

            _logger.LogInformation("Updated stats for user ID: {UserId} - TotalWorkouts: {Workouts}, Minutes: {Minutes}, Calories: {Calories}",
                userId, stats.TotalWorkouts, stats.TotalMinutes, stats.TotalCaloriesBurned);

            _context.Entry(stats).CurrentValues.SetValues(stats);
        }
        else
        {
            stats = new UserGeneralStatsModel
            {
                UserId = userId,
                TotalWorkouts = 1,
                TotalMinutes = duration,
                TotalCaloriesBurned = calories
            };
            _logger.LogInformation("Created new stats for user ID: {UserId}", userId);
            await _context.UserGeneralStats.AddAsync(stats);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Workout increment saved for user ID: {UserId}", userId);
    }

    public async Task UpdateWorkoutStreakAsync(Guid userId, DateTime workoutDate)
    {
        _logger.LogInformation("Updating workout streak for user ID: {UserId}", userId);
        var stats = await GetByUserIdAsync(userId);

        if (stats != null)
        {
            stats.CurrentWorkoutStreak = (int)(DateTime.UtcNow.Date - workoutDate.Date).TotalDays;
            _logger.LogInformation("Streak updated for user ID: {UserId} - New Streak: {StreakDays}", userId, stats.CurrentWorkoutStreak);

            _context.Entry(stats).CurrentValues.SetValues(stats);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("Cannot update streak: no stats found for user ID: {UserId}", userId);
        }
    }

    public async Task ResetStreakAsync(Guid userId)
    {
        _logger.LogInformation("Resetting workout streak for user ID: {UserId}", userId);
        var stats = await GetByUserIdAsync(userId);

        if (stats != null)
        {
            stats.CurrentWorkoutStreak = 0;
            _logger.LogInformation("Streak reset for user ID: {UserId}", userId);

            _context.Entry(stats).CurrentValues.SetValues(stats);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("No stats found to reset streak for user ID: {UserId}", userId);
        }
    }
}

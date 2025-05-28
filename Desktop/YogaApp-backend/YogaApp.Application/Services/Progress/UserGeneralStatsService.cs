using YogaApp.Application.Helpers;
using YogaApp.Application.Mappers.Progress;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.Progress;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Progress;

public class UserGeneralStatsService : IUserGeneralStatsService
{
    private readonly IUserWorkoutService _userWorkoutService;
    private readonly IUserGeneralStatsRepository _userStatsRepository;
    private readonly ILogger<UserGeneralStatsService> _logger;
    
    public UserGeneralStatsService(IUserWorkoutService userWorkoutService, IUserGeneralStatsRepository userStatsRepository,
        ILogger<UserGeneralStatsService> logger)
    {
        _logger = logger;
        _userStatsRepository = userStatsRepository;
        _userWorkoutService = userWorkoutService;
    }

    public async Task<UserGeneralStatsDto> GetGeneralStats(Guid? userId)
    {
        _logger.LogInformation("Getting general stats for user {UserId}", userId);
        var stats = await _userStatsRepository.GetByUserIdAsync(userId);

        if (stats == null)
        {
            _logger.LogWarning("No general stats found for user {UserId}. Returning empty stats.", userId);
            return new UserGeneralStatsDto
            {
                UserId = userId,
                TotalWorkouts = 0,
                TotalMinutes = 0,
                TotalCaloriesBurned = 0,
                CurrentWorkoutStreak = 0,
                LongestWorkoutStreak = 0
            };
        }

        _logger.LogInformation("General stats found for user {UserId}. Returning updated stats.", userId);
        var updatedStats = await _userStatsRepository.GetByUserIdAsync(userId);
        
        return updatedStats.ToUserGeneralStatsDto();
    }
    
    public UserGeneralStatsModel CalculateGeneralStats(List<UserWorkoutDto> workouts)
    {
        _logger.LogInformation("Calculating general stats from {Count} workouts", workouts.Count);
        var workoutList = workouts.ToList();

        return new UserGeneralStatsModel()
        {
            TotalWorkouts = workoutList.Count,
            TotalMinutes = workoutList.Sum(w => w.Duration),         
            TotalCaloriesBurned = workoutList.Sum(w => w.CaloriesBurned), 
            CurrentWorkoutStreak = StreakCalculator.CalculateCurrentWorkoutStreak(workoutList),
            LongestWorkoutStreak = StreakCalculator.CalculateLongestWorkoutStreak(workoutList)
        };
    }
    
    public async Task UpdateGeneralStats(Guid? userId)
    {
        _logger.LogInformation("Updating general stats for user {UserId}", userId);
        
        var workouts = await _userWorkoutService.GetCompletedWorkouts(userId);
        var calculated = CalculateGeneralStats(workouts);

        var stats = await _userStatsRepository.GetByUserIdAsync(userId) ?? new UserGeneralStatsModel { UserId = userId };

        stats.TotalWorkouts = calculated.TotalWorkouts;
        stats.TotalMinutes = calculated.TotalMinutes;
        stats.TotalCaloriesBurned = calculated.TotalCaloriesBurned;
        stats.CurrentWorkoutStreak = calculated.CurrentWorkoutStreak;
        stats.LongestWorkoutStreak = calculated.LongestWorkoutStreak;

        await _userStatsRepository.UpdateAsync(stats);
        
        _logger.LogInformation("General stats updated for user {UserId}", userId);
    }
}
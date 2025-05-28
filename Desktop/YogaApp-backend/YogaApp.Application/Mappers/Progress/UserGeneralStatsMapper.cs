using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Application.Mappers.Progress;

public static class UserGeneralStatsMapper
{
    public static UserGeneralStatsDto ToUserGeneralStatsDto(this UserGeneralStatsModel? userGeneralStats)
    {
        return new UserGeneralStatsDto()
        {
            UserId = userGeneralStats.UserId,
            TotalWorkouts = userGeneralStats.TotalWorkouts,
            TotalMinutes = userGeneralStats.TotalMinutes,
            TotalCaloriesBurned = userGeneralStats.TotalCaloriesBurned,
            CurrentWorkoutStreak = userGeneralStats.CurrentWorkoutStreak,
            LongestWorkoutStreak = userGeneralStats.LongestWorkoutStreak,
        };
    }
    
    public static UserGeneralStatsModel ToUserGeneralStatsModel(this UserGeneralStatsDto userGeneralStatsDto)
    {
        return new UserGeneralStatsModel()
        {
            UserId = userGeneralStatsDto.UserId,
            TotalWorkouts = userGeneralStatsDto.TotalWorkouts,
            TotalMinutes = userGeneralStatsDto.TotalMinutes,
            TotalCaloriesBurned = userGeneralStatsDto.TotalCaloriesBurned,
            CurrentWorkoutStreak = userGeneralStatsDto.CurrentWorkoutStreak,
            LongestWorkoutStreak = userGeneralStatsDto.LongestWorkoutStreak,
        };
    }
}
using YogaApp.Application.ServiceContact.DTO;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Domain.User;

namespace YogaApp.Application.Helpers;

public static class StreakCalculator
{
    public static int CalculateCurrentWorkoutStreak(IEnumerable<UserWorkoutDto> userWorkouts)
    {
        var performedDates = userWorkouts
            .Where(uw => uw.LastPerformedAt.HasValue)
            .Select(w => w.LastPerformedAt.Value.Date)
            .OrderByDescending(d => d) // Сортуємо по даті
            .Distinct() // Видаляємо дублікати дат
            .ToList();;

        if (performedDates.Count == 0)
            return 0;

        int streak = 1;
        DateTime today = DateTime.UtcNow.Date;

        // Якщо перший воркаут не сьогодні і не вчора, то стрік перервано
        if (performedDates[0] != today && performedDates[0] != today.AddDays(-1))
            return 0;

        // Перевіряємо наступні воркаути на наявність послідовності
        for (int i = 1; i < performedDates.Count; i++)
        {
            if (performedDates[i] == performedDates[i - 1].AddDays(-1))
                streak++;
            else
                break;
        }

        return streak;
    }

    public static int CalculateLongestWorkoutStreak(IEnumerable<UserWorkoutDto> userWorkouts)
    { 
        var performedDates = userWorkouts
            .Where(w => w.LastPerformedAt.HasValue)
            .Select(w => w.LastPerformedAt.Value.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        int maxStreak = 0, currentStreak = 1;

        for (int i = 1; i < performedDates.Count; i++)
        {
            if (performedDates[i] == performedDates[i - 1].AddDays(1)) // Якщо наступна дата на один день більша
            {
                currentStreak++;
            }
            else
            {
                maxStreak = Math.Max(maxStreak, currentStreak);
                currentStreak = 1;
            }
        }

        return Math.Max(maxStreak, currentStreak);
    }
}
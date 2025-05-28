using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Core.RepositoryContracts;

public interface IUserGeneralStatsRepository
{
    Task<UserGeneralStatsModel?> GetByUserIdAsync(Guid? userId);                 // Отримати загальну статистику користувача
    Task UpdateAsync(UserGeneralStatsModel stats);                             // Оновити всі показники
    Task IncrementWorkoutCountAsync(Guid userId, int duration, int calories); 
    Task UpdateWorkoutStreakAsync(Guid userId, DateTime workoutDate); // Оновити стрік
    Task ResetStreakAsync(Guid userId);  
}
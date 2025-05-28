using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Core.RepositoryContracts;

public interface IUserWeightTrackingRepository
{
    Task<UserWeightTrackingModel?> GetWeightStatsAsync(Guid? userId);
    Task<UserWeightTrackingModel?> GetByUserIdAsync(Guid? userId); // Отримати поточну інформацію про вагу користувача
    Task AddAsync(UserWeightTrackingModel tracking); // Додати новий запис (наприклад, при реєстрації)
    Task UpdateAsync(UserWeightTrackingModel tracking); // Оновити всю модель
//     Task UpdateWeightAsync(Guid userId, double newWeight); // Оновити тільки поточну вагу
//     Task AddWeightEntryAsync(Guid userId, double weight); // Додати запис до історії ваги
}
    
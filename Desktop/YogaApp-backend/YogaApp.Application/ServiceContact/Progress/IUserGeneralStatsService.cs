using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Domain.User.UserProgress;

namespace YogaApp.Application.ServiceContact.Progress;

public interface IUserGeneralStatsService
{
    Task<UserGeneralStatsDto> GetGeneralStats(Guid? userId);
    UserGeneralStatsModel CalculateGeneralStats(List<UserWorkoutDto> workouts);
    Task UpdateGeneralStats(Guid? userId);
}
using YogaApp.Application.Mappers.Workout;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Core.Domain.User;

namespace YogaApp.Application.Mappers.User;

public static class UserWorkoutMapper
{
    public static UserWorkoutDto ToUserWorkoutDto(this UserWorkoutModel model)
    {
        return new UserWorkoutDto
        {
            UserWorkoutId = model.UserWorkoutId,
            UserId = model.UserId,
            WorkoutId = model.WorkoutId,
            Duration = model.Duration,            
            CaloriesBurned = model.CaloriesBurned,
            UserName = model.User?.Name,
            StartedAt = model.StartedAt,
            CompletedAt = model.CompletedAt,
            Workout = model.Workout.ToWorkoutDto()
        };
    }

    public static UserWorkoutModel ToUserWorkoutModel(this UserWorkoutDto dto)
    {
        return new UserWorkoutModel()
        {
            UserWorkoutId = dto.UserWorkoutId,
            UserId = dto.UserId,
            WorkoutId = dto.WorkoutId,
            StartedAt = dto.StartedAt,
            CompletedAt = dto.CompletedAt,
            Workout = dto.Workout.ToWorkoutModel()
        };
    }
}
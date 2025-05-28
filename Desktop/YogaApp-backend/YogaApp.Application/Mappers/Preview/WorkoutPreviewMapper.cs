using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.Mappers.Preview;

public static class WorkoutPreviewMapper
{
    public static WorkoutPreviewDto ToWorkoutPreviewDto(this WorkoutModel workout)
    {
        return new WorkoutPreviewDto()
        {
            Name = workout.Name,
            Duration = workout.Duration,
            ImageUrl = workout.ImageUrl,
        };
    }
}
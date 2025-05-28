using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.Mappers.Workout;

public static class CustomWorkoutMapper
{
    // public static CustomWorkoutDto ToCustomWorkoutDto(this CustomWorkoutModel customWorkoutModel)
    // {
    //     return new CustomWorkoutDto()
    //     {
    //         CustomWorkoutId = customWorkoutModel.CustomWorkoutId,
    //         Name = customWorkoutModel.Name,
    //         Difficulty = customWorkoutModel.Difficulty,
    //         CaloriesBurned = customWorkoutModel.CaloriesBurned,
    //         Duration = customWorkoutModel.Duration,
    //         CreatedAt = customWorkoutModel.CreatedAt,
    //         Exercises = customWorkoutModel.Exercises?.Select(e => e.ToExerciseDto()).ToList()
    //     };
    // }

    // public static CustomWorkoutModel ToCustomWorkoutModel(this CustomWorkoutDto customWorkoutDto)
    // {
    //     return new CustomWorkoutModel()
    //     {
    //         CustomWorkoutId = customWorkoutDto.CustomWorkoutId,
    //         Name = customWorkoutDto.Name,
    //         Difficulty = customWorkoutDto.Difficulty,
    //         CreatedAt = customWorkoutDto.CreatedAt,
    //         CaloriesBurned = customWorkoutDto.CaloriesBurned,
    //         Duration = customWorkoutDto.Duration,
    //     };
    // }

    public static CustomWorkoutModel ToCustomWorkoutModel(this CustomWorkoutRequestDto customWorkoutRequestDto)
    {
        return new CustomWorkoutModel()
        {
            CustomWorkoutId = Guid.NewGuid(),
            UserId = customWorkoutRequestDto.UserId,
            Name = customWorkoutRequestDto.Name,
            ImageUrl = customWorkoutRequestDto.ImageUrl,
            Difficulty = customWorkoutRequestDto.Difficulty,
            CaloriesBurned = customWorkoutRequestDto.CaloriesBurned,
            Duration = customWorkoutRequestDto.Duration,
            Category = customWorkoutRequestDto.Category,
            Exercises = customWorkoutRequestDto.Exercises.Select(e => e.ToExerciseModel()).ToList()
        };
    }

    public static CustomWorkoutResponseDto ToCustomWorkoutResponseDto(this CustomWorkoutModel? customWorkoutModel)
    {
        return new CustomWorkoutResponseDto()
        {
            CustomWorkoutId = customWorkoutModel.CustomWorkoutId,
            Name = customWorkoutModel.Name,
            ImageUrl = customWorkoutModel.ImageUrl,
            Difficulty = customWorkoutModel.Difficulty,
            Category = customWorkoutModel.Category,
            CaloriesBurned = customWorkoutModel.CaloriesBurned,
            Duration = customWorkoutModel.Duration,
            CreatedAt = customWorkoutModel.CreatedAt,
            Exercises = customWorkoutModel.Exercises?.Select(e => e.ToExerciseDto()).ToList() 
        };
    }
    public static CustomWorkoutPreviewDto ToCustomWorkoutPreviewDto(this CustomWorkoutModel model)
    {
        return new CustomWorkoutPreviewDto
        {
            Name = model.Name,
            ImageUrl = model.ImageUrl
        };
    }

}
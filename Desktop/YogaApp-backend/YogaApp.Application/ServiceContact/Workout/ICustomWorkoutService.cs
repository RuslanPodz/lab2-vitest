using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.ServiceContact.Workout;

public interface ICustomWorkoutService : IBaseWorkoutService<CustomWorkoutPreviewDto, CustomWorkoutResponseDto, CustomWorkoutRequestDto>
{
    Task<List<CustomWorkoutPreviewDto>> GetAllCustomWorkoutPreviewsAsync();

}
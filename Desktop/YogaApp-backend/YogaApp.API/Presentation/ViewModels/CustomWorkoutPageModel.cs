using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.ServiceContact.DTO;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;

namespace MobileYogaApp.Presentation.ViewModels;

public class CustomWorkoutPageModel
{
    public string Title { get; set; } = "Custom";
    public List<CustomWorkoutPreviewDto> CustomWorkouts { get; set; } = new();
}
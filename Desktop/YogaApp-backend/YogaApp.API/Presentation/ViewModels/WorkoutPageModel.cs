using YogaApp.Application.ServiceContact.DTO;
using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace MobileYogaApp.Presentation.ViewModels;

public class WorkoutPageModel
{
    public string? UserName { get; set; }
    public string? SearchQuery { get; set; } 
    public List<WorkoutDto>? FavouriteWorkouts { get; set; } 
    
    public List<WorkoutDto>? RecentWorkouts { get; set; } 
    public Dictionary<string, List<WorkoutPreviewDto>?> WorkoutsByCategory { get; set; }
}
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Core.Domain.User.UserProgress;

namespace MobileYogaApp.Presentation.ViewModels.ProgressPageModel;

public class GeneralStats
{
    public string Title { get; set; } = "Totals";
    public  UserGeneralStatsDto? Stats { get; set; }
    
}
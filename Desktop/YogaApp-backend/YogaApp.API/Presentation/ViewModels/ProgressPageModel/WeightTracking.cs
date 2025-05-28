using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;
using YogaApp.Core.Domain.User.UserProgress;

namespace MobileYogaApp.Presentation.ViewModels.ProgressPageModel;

public class WeightTracking
{
    public string Title { get; set; } = "Weight";
    public UserWeightTrackingDto? UserWeight { get; set; }
}

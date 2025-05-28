using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;

namespace YogaApp.Application.ServiceContact.Progress;

public interface IUserWeightTrackingService
{
    Task<UserWeightTrackingDto> GetWeightStatsAsync(Guid? userId);

    // Task UpdateWeightAsync(Guid? userId, UserWeightTrackingUpdateDataDto? request);
    
    Task AddUserInitialWeightAsync(Guid? userId, UserWeightTrackingInitialDataDto request);

    Task<double> UpdateWeightAndCalculateBMIAsync(Guid? userId, double newWeight, double heightInMeters);
}
using YogaApp.Application.Helpers;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.Enums;

namespace YogaApp.Application.Mappers.Progress;

public static class UserWeightTrackingMapper
{
    public static UserWeightTrackingDto ToUserWeightTrackingDto(this UserWeightTrackingModel? userWeightTracking)
    {
        if (userWeightTracking == null)
            throw new ArgumentNullException(nameof(userWeightTracking));
        
        return new UserWeightTrackingDto()
        {
            UserId = userWeightTracking.UserId,
            WeightTrackingId = userWeightTracking.WeightTrackingId,
            StartWeight = userWeightTracking.StartWeight,
            Height = userWeightTracking.Height,
            CurrentWeight = userWeightTracking.CurrentWeight,
            DifferenceInWeight = userWeightTracking.DifferenceInWeight,
            BMI = BMICalculator.CalculateBMI(userWeightTracking.CurrentWeight, userWeightTracking.Height),
            BMIStatus = userWeightTracking.BMIStatus
        };
    }
    
    public static UserWeightTrackingModel ToUserWeightTrackingModel(this UserWeightTrackingDto userWeightTrackingDto)
    {
        return new UserWeightTrackingModel()
        {
            UserId = userWeightTrackingDto.UserId,
            StartWeight = userWeightTrackingDto.StartWeight,
            CurrentWeight = userWeightTrackingDto.CurrentWeight,
            Height = userWeightTrackingDto.Height,
            BMIStatus = userWeightTrackingDto.BMIStatus
        };
    }

    public static UserWeightTrackingDto ToUserWeightTrackingDto(this UserCalculateBmiRequest request)
    {
        double bmi = BMICalculator.CalculateBMI(request.Weight, request.HeightInMeters);
        
        BMIStatus bmiStatus = BMICalculator.CalculateBmiStatus(bmi);

        return new UserWeightTrackingDto()
        {
            CurrentWeight = request.Weight,
            Height = request.HeightInMeters,
            BMI = bmi,
            BMIStatus = bmiStatus
        };
    } 
    
    public static UserWeightTrackingModel ToUserWeightTrackingModel(this UserWeightTrackingInitialDataDto dto)
    {
        return new UserWeightTrackingModel
        {
            StartWeight = dto.StartWeight,
            Height = dto.Height,
        };
    }
    
    public static UserWeightTrackingDto ToUserWeightTrackingUpdateDto(this UserWeightTrackingUpdateDataDto dto)
    {
        return new UserWeightTrackingDto
        {
            CurrentWeight = dto.CurrentWeight,
            Height = dto.Height,
        };
    }
}
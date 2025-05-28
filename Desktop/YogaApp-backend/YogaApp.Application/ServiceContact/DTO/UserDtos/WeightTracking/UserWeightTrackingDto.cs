using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;

public class UserWeightTrackingDto
{
    public Guid? UserId { get; set; }
    public Guid? WeightTrackingId { get; set; }
    public double StartWeight { get; set; }
    public double CurrentWeight { get; set; }
    public double DifferenceInWeight { get; set; } // Розраховане до мапінгу
    public double Height { get; set; }
    public double BMI { get; set; } // Також розраховане
    public BMIStatus BMIStatus { get; set; } = BMIStatus.Healthy;
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.User.UserProgress;

public class UserWeightTrackingModel
{
    [Key]
    public Guid? WeightTrackingId { get; set; }

    [Required]
    public Guid? UserId { get; set; }
 
    public double StartWeight { get; set; }
    public double CurrentWeight { get; set; }
    public double DifferenceInWeight { get; set; } 
    public double Height { get; set; }
    
    public double BMI { get; set; }
    public BMIStatus BMIStatus { get; set; } = BMIStatus.Healthy;
    
    public UserModel? User { get; set; }
  
}
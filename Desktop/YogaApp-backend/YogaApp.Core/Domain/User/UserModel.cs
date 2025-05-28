using System.ComponentModel.DataAnnotations;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Core.Domain.User;

public class UserModel
{
    [Key]
    public Guid UserId  { get; set; }
    
    [StringLength(50)]
    public string Name  { get; set; } = string.Empty;
    
    [StringLength(250)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(250)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public List<UserProgramModel>? UserPrograms { get; set; } = new();
    public List<UserWorkoutModel>? UserWorkouts { get; set; } = new();
    public UserGeneralStatsModel? GeneralStats { get; set; }
    public UserWeightTrackingModel? WeightTracking { get; set; }
    public List<CustomWorkoutModel>? CustomWorkouts { get; set; } = new();

}
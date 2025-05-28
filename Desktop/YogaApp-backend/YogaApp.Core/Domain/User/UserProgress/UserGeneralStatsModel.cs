using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YogaApp.Core.Domain.User.UserProgress;

public class UserGeneralStatsModel
{
    [Key, ForeignKey("User")]
    public Guid? UserId { get; set; }
    public int TotalWorkouts { get; set; }
    public int? TotalMinutes { get; set; }
    public int? TotalCaloriesBurned { get; set; }
    
    public int CurrentWorkoutStreak { get; set; }
    public int LongestWorkoutStreak { get; set; }
    public UserModel? User { get; set; }

}
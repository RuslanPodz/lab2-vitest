using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.User;

public class UserWorkoutModel
{
    [Key]
    public Guid UserWorkoutId { get; set; }
    
    [ForeignKey("User")]
    public Guid? UserId { get; set; }
    
    [ForeignKey("Workout")]
    public Guid WorkoutId { get; set; }
    public int Duration { get; set; }
    public int CaloriesBurned { get; set; }
    
    [StringLength(50)]
    public string? UserName { get; set; }
    public bool IsFavourite { get; set; } = false;

    public WorkoutStatus Status { get; set; } = WorkoutStatus.NotStarted;
    public DateTime? LastPerformedAt { get; set; } 
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public WorkoutModel? Workout { get; set; }
    public UserModel? User { get; set; }
}
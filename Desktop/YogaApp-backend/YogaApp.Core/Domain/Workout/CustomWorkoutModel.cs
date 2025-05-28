using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.Workout;

public class CustomWorkoutModel
{
    [Key]
    public Guid CustomWorkoutId { get; set; }
    public string? ImageUrl { get; set; }
    
    [Required]
    [StringLength(60)]
    public string? Name { get; set; }
    
    [StringLength(30)]
    public string? Category { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy;     
    public int? Duration { get; set; } // 
    public int? CaloriesBurned { get; set; }
    public List<ExerciseModel>? Exercises { get; set; }
    
    [ForeignKey("User")]
    public Guid? UserId { get; set; } 
    public UserModel? User { get; set; }

}
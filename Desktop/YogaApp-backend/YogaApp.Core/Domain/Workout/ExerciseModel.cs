using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.Workout;

public class ExerciseModel
{
    [Key]
    public Guid ExerciseId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsLiked { get; set; }
    public int Duration { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(250)]
    public string? Description { get; set; }
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy; 
    
    public ICollection<WorkoutModel> Workouts { get; set; } = new List<WorkoutModel>();
    public ICollection<CustomWorkoutModel> CustomWorkouts { get; set; } = new List<CustomWorkoutModel>();
}
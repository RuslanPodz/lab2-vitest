using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Domain.Program;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.Workout;

public class WorkoutModel
{
    [Key]
    public Guid WorkoutId { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFavourite { get; set; }
    public DateTime? LastPerformedAt { get; set; }
    
    // [Required]
    [StringLength(50)]
    public string? Name { get; set; }
    
    [StringLength(50)]
    public string? Category { get; set; }
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy; 
    public int? Duration { get; set; } // 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? CaloriesBurned { get; set; }
    // public WorkoutSettingsModel WorkoutSettingsModel { get; set; }
    // public MusicSettingsModel MusicSettingsModel { get; set; }
    
    public ICollection<ExerciseModel>? Exercises { get; set; }
    public ICollection<UserWorkoutModel> UserWorkouts { get; set; } = new List<UserWorkoutModel>();
    // In WorkoutModel.cs
    public ICollection<ProgramModel> Programs { get; set; } = new List<ProgramModel>();
}
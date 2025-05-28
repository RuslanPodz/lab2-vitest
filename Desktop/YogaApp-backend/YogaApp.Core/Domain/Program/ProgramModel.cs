using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YogaApp.Core.Domain.User;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.Enums;

namespace YogaApp.Core.Domain.Program;

public class ProgramModel
{
    [Key]
    public Guid ProgramId { get; set; } 
    
    [StringLength(60)]
    public string? ProgramName { get; set; }
    
    [StringLength(200)]
    public string? ProgramDescription { get; set; }
    
    [StringLength(50)]
    public string ProgramCategory { get; set; } = string.Empty;
    public string? ProgramImage { get; set; }
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy; 
    // public WorkoutSettingsModel WorkoutSettingsModel { get; set; }
    // public MusicSettingsModel MusicSettingsModel { get; set; }
    public ICollection<WorkoutModel>? Workouts { get; set; } = new List<WorkoutModel>();
    public List<UserProgramModel>? UserPrograms { get; set; }
    
    [ForeignKey("User")]
    public Guid? UserId { get; set; }
    public UserModel? User { get; set; }
}
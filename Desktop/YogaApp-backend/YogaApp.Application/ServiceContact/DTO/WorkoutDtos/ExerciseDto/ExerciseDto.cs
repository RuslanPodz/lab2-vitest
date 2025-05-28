using System.ComponentModel.DataAnnotations;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

public class ExerciseDto
{
    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    [Required] public string Description { get; set; } = string.Empty;
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy; 
}

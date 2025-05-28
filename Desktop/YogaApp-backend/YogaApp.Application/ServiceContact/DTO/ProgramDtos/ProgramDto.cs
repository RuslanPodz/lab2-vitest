using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.ProgramDtos;

public class ProgramDto
{
    public Guid ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public string? ProgramDescription { get; set; }
    public string? ProgramImage { get; set; }
    public WorkoutDifficulty Difficulty { get; set; } = WorkoutDifficulty.Easy; 
    public List<WorkoutDto> Workouts { get; set; }
}
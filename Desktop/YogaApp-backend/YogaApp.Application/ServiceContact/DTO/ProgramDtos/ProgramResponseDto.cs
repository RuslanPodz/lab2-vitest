using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.ProgramDtos;

public class ProgramResponseDto
{
    public Guid ProgramId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public WorkoutDifficulty Difficulty { get; set; }
    public List<WorkoutResponseDto>? Workouts { get; set; }
}
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

public class ExerciseResponseDto
{
    public Guid ExerciseId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public WorkoutDifficulty Difficulty { get; set; }
}
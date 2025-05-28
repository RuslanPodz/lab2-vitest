using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;

public class CustomWorkoutDto
{
    public Guid CustomWorkoutId { get; set; }
    public string? Name { get; set; } = String.Empty;
    public WorkoutDifficulty Difficulty { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? Duration { get; set; }
    public int? CaloriesBurned { get; set; }
    public List<ExerciseDto.ExerciseDto>? Exercises { get; set; }
}
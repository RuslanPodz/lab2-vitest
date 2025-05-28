using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;

public class CustomWorkoutResponseDto
{
        public Guid CustomWorkoutId { get; set; }
        public string? Name { get; set; }
        public WorkoutDifficulty Difficulty { get; set; }
        public string? ImageUrl { get; set; }
        public int? Duration { get; set; }
        public int? CaloriesBurned { get; set; }
        public string? Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<ExerciseDto.ExerciseDto>? Exercises { get; set; } = new();
}
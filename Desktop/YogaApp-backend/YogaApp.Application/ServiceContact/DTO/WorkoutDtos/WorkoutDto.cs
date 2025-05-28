using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

public class WorkoutDto : IWorkoutPerformed
{
        public Guid WorkoutId { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFavourite { get; set; }
        public WorkoutDifficulty Difficulty { get; set; }
        public DateTime? LastPerformedAt { get; set; }
        public int? Duration { get; set; }
        public string? Category { get; set; }
        public int? CaloriesBurned { get; set; }
        public List<ExerciseDto.ExerciseDto> Exercises { get; set; }
}
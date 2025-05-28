using System.Text.Json.Serialization;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;

public class CustomWorkoutRequestDto
{
    public Guid? UserId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkoutDifficulty Difficulty { get; set; }
    public int? Duration { get; set; }
    public int? CaloriesBurned { get; set; }
    public string? Category { get; set; }
    public List<ExerciseDto.ExerciseDto> Exercises { get; set; } = new();
    
}
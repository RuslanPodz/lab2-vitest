using System.Text.Json.Serialization;
using YogaApp.Core.Enums;

namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

public class ExerciseRequestDto
{
    public string Name { get; set; } 
    public string? Description { get; set; } 
    public int Duration { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkoutDifficulty Difficulty { get; set; }
}
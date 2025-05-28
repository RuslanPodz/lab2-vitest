using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace YogaApp.Application.ServiceContact.DTO.ProgramDtos;

public class AddWorkoutToProgramRequest
{
    public WorkoutRequestDto? Workout { get; set; } = new();
    public string? ExistingWorkoutName { get; set; }
}
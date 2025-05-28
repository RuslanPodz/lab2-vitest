using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace YogaApp.Application.ServiceContact.DTO.ProgramDtos;

public class ProgramWorkoutsDto
{
    public Guid ProgramId { get; set; }
    public List<WorkoutDto> Workouts { get; set; } = new();
}
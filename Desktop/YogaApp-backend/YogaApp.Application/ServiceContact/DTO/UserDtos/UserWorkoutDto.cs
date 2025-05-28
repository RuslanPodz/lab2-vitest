using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.Workout;

namespace YogaApp.Application.ServiceContact.DTO.UserDtos;

public class UserWorkoutDto : IWorkoutPerformed
{
    public Guid UserWorkoutId { get; set; }
    public Guid? UserId { get; set; }
    public Guid WorkoutId { get; set; }
    public int? Duration { get; set; }
    public int? CaloriesBurned { get; set; }
    public string? UserName { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public WorkoutDto Workout { get; set; }
    public UserResponseDto User { get; set; }

    public DateTime? LastPerformedAt { get; }
}
using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.CustomWorkoutDto;

namespace YogaApp.Application.ServiceContact.DTO.UserDtos;

public class UserResponseDto
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    // public List<UserProgramsD>? UserPrograms { get; set; }
    public List<UserWorkoutDto>? UserWorkouts { get; set; }
    public List<CustomWorkoutDto>? CustomWorkouts { get; set; }
    public List<UserGeneralStatsDto>? UserGeneralStats { get; set; }
    public List<UserWeightTrackingDto>? UserWeightTrackings { get; set; }
}
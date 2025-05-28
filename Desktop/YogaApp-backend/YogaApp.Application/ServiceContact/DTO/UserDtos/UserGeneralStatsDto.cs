namespace YogaApp.Application.ServiceContact.DTO.UserDtos;

public class UserGeneralStatsDto
{
    public Guid? UserId { get; set; }
    public int TotalWorkouts { get; set; }
    public int? TotalMinutes { get; set; }
    public int? TotalCaloriesBurned { get; set; }
    
    public int CurrentWorkoutStreak { get; set; }
    public int LongestWorkoutStreak { get; set; }
}
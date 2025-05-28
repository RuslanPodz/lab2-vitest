namespace YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;

public class AddExerciseToWorkoutDto
{
    public ExerciseRequestDto? Exercise { get; set; } = new();
    public string? ExistingExerciseName { get; set; } = string.Empty;
     
}
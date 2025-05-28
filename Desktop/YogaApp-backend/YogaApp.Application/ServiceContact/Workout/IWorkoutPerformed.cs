namespace YogaApp.Application.ServiceContact.Workout;

public interface IWorkoutPerformed
{
    DateTime? LastPerformedAt { get; }
    int? Duration { get; }
    int? CaloriesBurned { get; }
}
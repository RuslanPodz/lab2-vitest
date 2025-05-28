using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Application.ServiceContact.Workout;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.Mappers.Workout;

public static class WorkoutMapper
{
    public static WorkoutDto ToWorkoutDto(this WorkoutModel workout)
    {
        if (workout == null)
            throw new ArgumentNullException(nameof(workout), "WorkoutModel is null in ToWorkoutDto.");
        
        return new WorkoutDto()
        {
            WorkoutId = workout.WorkoutId,
            Name = workout.Name,
            ImageUrl = workout.ImageUrl,
            IsFavourite = workout.IsFavourite,
            LastPerformedAt = workout.LastPerformedAt,
            Difficulty = workout.Difficulty,
            Category = workout.Category,
            CaloriesBurned = workout.CaloriesBurned,
            Duration = workout.Duration,
            Exercises = workout.Exercises?
                .Select(e => e.ToExerciseDto())
                .ToList() ?? []
        };
    }
    
    public static WorkoutModel ToWorkoutModel(this WorkoutDto dto)
    {
        return new WorkoutModel
        {
            WorkoutId = dto.WorkoutId,
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            IsFavourite = dto.IsFavourite,
            LastPerformedAt = dto.LastPerformedAt,
            Difficulty = dto.Difficulty,
            Category = dto.Category,
            CaloriesBurned = dto.CaloriesBurned,
            Duration = dto.Duration,
            Exercises = dto.Exercises.Select(e => e.ToExerciseModel()).ToList()
        };
    }
    
    public static WorkoutModel ToWorkoutModel(this WorkoutRequestDto dto)
    {
        return new WorkoutModel
        {
            WorkoutId = Guid.NewGuid(), 
            Name = dto.Name,
            ImageUrl = dto.ImageUrl,
            Difficulty = dto.Difficulty,
            Duration = dto.Duration,
            CaloriesBurned = dto.CaloriesBurned,
            Category = dto.Category,
            Exercises = dto.Exercises.Select(e => e.ToExerciseModel()).ToList()
        };
    }
    
    public static WorkoutResponseDto ToWorkoutResponseDto(this WorkoutModel? model)
    {
        if (model == null)
        {
           throw new ArgumentNullException(nameof(model));
        }
        return new WorkoutResponseDto
        {
            WorkoutId = model.WorkoutId,
            Name = model.Name,
            Difficulty = model.Difficulty,
            Duration = model.Duration,
            LastPerformedAt = model.LastPerformedAt,
            CaloriesBurned = model.CaloriesBurned,
            Category = model.Category,
            CreatedAt = model.CreatedAt,
            Exercises = model.Exercises != null 
                ? model.Exercises.Select(e => e.ToExerciseDto()).ToList()
                : new List<ExerciseDto>()        };
    }
    
    public static IWorkoutPerformed MapToWorkoutPerformed(WorkoutDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        
        return new WorkoutDto()
        {
            Name = dto.Name,
            Duration = dto.Duration,
            CaloriesBurned = dto.CaloriesBurned,
        };
    }
}
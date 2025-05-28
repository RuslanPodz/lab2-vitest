using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos.ExerciseDto;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.Mappers.Workout;

public static class ExerciseMapper
{
    public static ExerciseDto ToExerciseDto(this ExerciseModel? exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));
        
        return new ExerciseDto()
        {
            Name = exercise.Name,
            Description = exercise.Description,
            Difficulty = exercise.Difficulty,
            Duration = exercise.Duration,
        };
    }
    public static ExerciseModel ToExerciseModel(this ExerciseDto exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));
        
        return new ExerciseModel()
        {
            Name = exercise.Name,
            Difficulty = exercise.Difficulty,
            Description = exercise.Description,
            Duration = exercise.Duration,
        };
    }
    
    public static ExerciseModel ToExerciseModel (this ExerciseRequestDto exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));
        
        return new ExerciseModel()
        {
            ExerciseId = Guid.NewGuid(),
            Name = exercise.Name,
            Description = exercise.Description,
            Difficulty = exercise.Difficulty,
            Duration = exercise.Duration,
        };
    }
    
    public static ExerciseRequestDto ToExerciseRequestDto(this ExerciseModel exercise)
    {
        return new ExerciseRequestDto()
        {
            Name = exercise.Name,
            Description = exercise.Description,
            Difficulty = exercise.Difficulty,
            Duration = exercise.Duration,
        };
    }
    public static ExerciseResponseDto ToExerciseResponseDto(this ExerciseModel exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));

        return new ExerciseResponseDto
        {
            ExerciseId = exercise.ExerciseId,
            Name = exercise.Name,
            Description = exercise.Description,
            Difficulty = exercise.Difficulty,
            Duration = exercise.Duration,
        };
    }
    
}
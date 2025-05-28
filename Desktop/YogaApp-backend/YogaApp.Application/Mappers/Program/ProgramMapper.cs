using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Core.Domain.Program;
using YogaApp.Core.Domain.Workout;

namespace YogaApp.Application.Mappers.Program;

public static class ProgramMapper
{
    public static ProgramDto ToProgramDto(this ProgramModel programModel)
    {
        return new ProgramDto()
        {
            ProgramId = programModel.ProgramId,
            ProgramName = programModel.ProgramName,
            ProgramDescription = programModel.ProgramDescription,
            ProgramImage = programModel.ProgramImage,
            Difficulty = programModel.Difficulty,
            Workouts = programModel.Workouts?.Select(w => new WorkoutDto
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                Difficulty = w.Difficulty,
                Duration = w.Duration,
                CaloriesBurned = w.CaloriesBurned
            }).ToList() ?? new List<WorkoutDto>()
        };
    }
    
    public static ProgramModel ToProgramModel(this ProgramDto programDto)
    {
        return new ProgramModel()
        {
            ProgramId = programDto.ProgramId,
            ProgramName = programDto.ProgramName,
            ProgramDescription = programDto.ProgramDescription,
            ProgramImage = programDto.ProgramImage,
            Difficulty = programDto.Difficulty,
            Workouts = programDto.Workouts.Select(w => new WorkoutModel
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                Difficulty = w.Difficulty,
                Duration = w.Duration,
                CaloriesBurned = w.CaloriesBurned
            }).ToList()
        };
    }
    
    public static ProgramModel ToProgramModel(this ProgramRequestDto programRequestDto)
    {
        return new ProgramModel()
        {
            ProgramId = Guid.NewGuid(),
            ProgramName = programRequestDto.Name,
            ProgramDescription = programRequestDto.Description,
            ProgramImage = programRequestDto.ImageUrl,
            ProgramCategory = programRequestDto.Category,
            // Workouts = programRequestDto.Workouts.Select(w => new WorkoutModel()
            // {
            //     // WorkoutId = w.WorkoutId,
            //     Name = w.Name,
            //     Difficulty = w.Difficulty,
            //     Duration = w.Duration,
            //     CaloriesBurned = w.CaloriesBurned
            // }).ToList()
        };
    }
    
    public static ProgramResponseDto ToProgramResponseDto(this ProgramModel? programModel)
    {
        if (programModel == null) throw new ArgumentNullException(nameof(programModel));
        
        return new ProgramResponseDto()
        {
            ProgramId = programModel.ProgramId,
            Name = programModel.ProgramName,
            Category = programModel.ProgramCategory,
            ImageUrl = programModel.ProgramImage,
            Difficulty = programModel.Difficulty,
            Workouts = programModel.Workouts?.Select(w => new WorkoutResponseDto
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                Difficulty = w.Difficulty,
                Duration = w.Duration,
                CaloriesBurned = w.CaloriesBurned
            }).ToList() ?? new List<WorkoutResponseDto>()
        };
    }
    
}
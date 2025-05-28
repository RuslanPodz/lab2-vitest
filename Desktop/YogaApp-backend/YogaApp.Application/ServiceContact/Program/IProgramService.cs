using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;

namespace YogaApp.Application.ServiceContact.Program;

public interface IProgramService
{
    Task<List<ProgramPreviewDto>> GetAllProgramsPreviewAsync();
    Task<ProgramWorkoutsDto> GetProgramWorkoutsAsync (Guid programId);  
    Task<ProgramResponseDto> GetProgramByIdAsync(Guid id);
    Task<List<ProgramResponseDto>> GetProgramsForUserAsync(Guid? userId);
    Task<bool> AddWorkoutToProgramAsync(Guid programId, WorkoutRequestDto workoutDto, string? existingWorkoutName); 
    Task<ProgramResponseDto> CreateProgramAsync(ProgramRequestDto program);  
    Task<ProgramResponseDto> UpdateProgramAsync(ProgramRequestDto program);  
    Task<bool> DeleteProgramAsync(Guid id);  
    Task JoinProgramAsync(Guid? userId, Guid programId);  
    Task<List<ProgramResponseDto>> GetProgramsByCategoryAsync(string categoryName);
    Task LeaveProgramAsync(Guid? userId, Guid programId);
}
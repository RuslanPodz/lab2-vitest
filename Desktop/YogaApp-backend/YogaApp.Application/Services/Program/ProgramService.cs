using System.Diagnostics;
using YogaApp.Application.Mappers.Preview;
using YogaApp.Application.Mappers.Program;
using YogaApp.Application.ServiceContact.DTO.Preview;
using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.WorkoutDtos;
using YogaApp.Application.ServiceContact.Program;
using YogaApp.Core.Domain.Workout;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Program;

public class ProgramService : IProgramService
{
    private readonly IProgramRepository _programRepository;
    private readonly IUserProgramRepository _userProgramRepo;
    private readonly IWorkoutRepository _workoutRepository;
    private readonly ILogger<ProgramService> _logger;

    public ProgramService(IProgramRepository programRepository, 
        IUserProgramRepository userProgramRepository,
        IWorkoutRepository workoutRepository,
        ILogger<ProgramService> logger)
    {
        _programRepository = programRepository;
        _userProgramRepo = userProgramRepository;
        _workoutRepository = workoutRepository;
        _logger = logger;
    }

    public async Task<List<ProgramPreviewDto>> GetAllProgramsPreviewAsync()
    {
        _logger.LogInformation("Get all program previews");
        var programs = await _userProgramRepo.GetAllPrograms();
        
        _logger.LogInformation("Retrieved {Count} programs", programs.Count);
        return programs.Select(p => p.ToProgramPreviewDto()).ToList();
    }

    public async Task<ProgramWorkoutsDto> GetProgramWorkoutsAsync (Guid programId)
    {
        _logger.LogInformation("Get workouts for program {ProgramId}", programId);
        var program = await _programRepository.GetProgramById(programId);
        if (program == null)
        {
            _logger.LogWarning("Program {ProgramId} not found", programId);
            throw new KeyNotFoundException("Program not found");
        }

        var workouts = program.Workouts ?? new List<WorkoutModel>();
        _logger.LogInformation("Program {ProgramId} has {Count} workouts", programId, workouts.Count);

        return new ProgramWorkoutsDto
        {
            ProgramId = program.ProgramId,
            Workouts = workouts.Select(w => new WorkoutDto
            {
                WorkoutId = w.WorkoutId,
                Name = w.Name,
                Duration = w.Duration,
                Difficulty = w.Difficulty,
                CaloriesBurned = w.CaloriesBurned
            }).ToList()
        };
    }

    public async Task<ProgramResponseDto> GetProgramByIdAsync(Guid id)
    {
        _logger.LogInformation("Get program by id {ProgramId}", id);
        var program =  await _programRepository.GetProgramById(id);
        
        _logger.LogInformation("Retrieved program {ProgramId}", id);
        return program.ToProgramResponseDto();
    }

    public async Task<List<ProgramResponseDto>> GetProgramsForUserAsync(Guid? userId)
    {
        _logger.LogInformation("Get programs for user {UserId}", userId);
        var userPrograms = await _userProgramRepo.GetProgramsForUser(userId);
        
        _logger.LogInformation("Retrieved {Count} programs for user {UserId}", userPrograms.Count, userId);
        return userPrograms.Select(p => p.ToProgramResponseDto()).ToList();
    }

    public async Task<bool> AddWorkoutToProgramAsync(Guid programId, WorkoutRequestDto workoutDto, string? existingWorkoutName)
    {
        _logger.LogInformation("Adding workout to program {ProgramId}", programId);
        var program = await _programRepository.GetProgramById(programId);
        if (program == null)
        {
            _logger.LogWarning("Program {ProgramId} not found", programId);
            throw new KeyNotFoundException("Program not found");
        }

        WorkoutModel? workout;

        if (existingWorkoutName != null)
        {
            workout = await _workoutRepository.GetWorkoutByNameAsync(existingWorkoutName);
            if (workout == null)
            {
                _logger.LogWarning("Workout with name '{WorkoutName}' not found", existingWorkoutName);
                throw new ArgumentException("Workout not found with that name");
            }
            _logger.LogInformation("Using existing workout '{WorkoutName}'", existingWorkoutName);
        }
        else
        {
            workout = new WorkoutModel
            {
                WorkoutId = Guid.NewGuid(),
                Name = workoutDto.Name,
                Duration = workoutDto.Duration,
                Difficulty = workoutDto.Difficulty,
                CaloriesBurned = workoutDto.CaloriesBurned
            };
            await _workoutRepository.AddWorkoutAsync(workout);
            _logger.LogInformation("Created new workout '{WorkoutName}'", workout.Name);
        }

        if (program.Workouts != null && program.Workouts.Any(w => w.WorkoutId == workout.WorkoutId))
        {
            _logger.LogWarning("Workout {WorkoutId} already exists in program {ProgramId}", workout.WorkoutId, programId);
            throw new InvalidOperationException("Workout already exists in the program.");
        }

        program.Workouts?.Add(workout);
        await _programRepository.UpdateProgram(program);

        _logger.LogInformation("Workout {WorkoutId} added to program {ProgramId}", workout.WorkoutId, programId);

        return true;
    }
    
    public async Task<ProgramResponseDto> CreateProgramAsync(ProgramRequestDto program)
    {
        _logger.LogInformation("Creating a new program");
        if (program == null)
        {
            _logger.LogError("Program data is required");
            throw new ArgumentNullException(nameof(program), "Program data is required.");
        }

        var newProgram = await _programRepository.CreateProgram(program.ToProgramModel());
        if (newProgram == null)
        {
            _logger.LogError("Failed to create program");
            throw new InvalidOperationException("Failed to create program.");
        }
        
        _logger.LogInformation("Program created with ID: {ProgramId}", newProgram.ProgramId);
        
        return newProgram.ToProgramResponseDto();
    }

    public async Task<ProgramResponseDto> UpdateProgramAsync(ProgramRequestDto program)
    {
        _logger.LogInformation("Updating program with name: {programName}", program.Name);
        var updatedProgram = await _programRepository.UpdateProgram(program.ToProgramModel());
        
        if (updatedProgram == null)
        {
            _logger.LogError("Failed to update program");
            throw new InvalidOperationException("Failed to update program.");
        }
        
        _logger.LogInformation("Program {ProgramId} updated successfully", updatedProgram.ProgramId);
        return updatedProgram.ToProgramResponseDto();
    }
    
    public async Task<bool> DeleteProgramAsync(Guid id)
    {
        _logger.LogInformation("Attempting to delete program with ID: {ProgramId}", id);

        var program = await _programRepository.GetProgramById(id);
        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found", id);
            throw new KeyNotFoundException("Program not found");
        }

        program.Workouts?.Clear();
        await _programRepository.UpdateProgram(program); 
        await _programRepository.DeleteProgram(id); 

        _logger.LogInformation("Program {ProgramId} deleted successfully", id);
        return true;
    }
    
    public async Task JoinProgramAsync(Guid? userId, Guid programId)
    {
        _logger.LogInformation("User {UserId} is joining program {ProgramId}", userId, programId);
        
        await _userProgramRepo.JoinAsync(userId, programId);
        
        _logger.LogInformation("User {UserId} successfully joined program {ProgramId}", userId, programId);
    }


    public async Task<List<ProgramResponseDto>> GetProgramsByCategoryAsync(string categoryName)
    {
        _logger.LogInformation("Fetching programs by category: {CategoryName}", categoryName);
        var programs = await _programRepository.GetProgramsByCategory(categoryName);
        
        _logger.LogInformation("Retrieved {Count} programs for category: {CategoryName}", programs.Count, categoryName);
        return programs.Select(p => p.ToProgramResponseDto()).ToList();
    }

    public async Task LeaveProgramAsync(Guid? userId, Guid programId)
    {
        _logger.LogInformation("User {UserId} is leaving program {ProgramId}", userId, programId);
        
        await _userProgramRepo.LeaveAsync(userId, programId);
        
        _logger.LogInformation("User {UserId} successfully leaved program {ProgramId}", userId, programId);
    }
}
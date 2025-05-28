using YogaApp.Application.Mappers;
using YogaApp.Application.Mappers.Program;
using YogaApp.Application.Mappers.User;
using YogaApp.Application.ServiceContact;
using YogaApp.Application.ServiceContact.DTO;
using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.User;

public class UserProgramService : IUserProgramService
{
    private readonly IUserProgramRepository _userProgramRepository;
    private readonly ILogger<UserProgramService>  _logger; 
    public UserProgramService(IUserProgramRepository userProgramRepository,
        ILogger<UserProgramService> logger)
    {
        _logger = logger;
        _userProgramRepository = userProgramRepository;
    }
    public async Task<UserProgramDto> JoinAsync(Guid userId, Guid programId)
    {
        _logger.LogInformation("Joining user with ID {UserId} to program", userId);
        var joinProgram = await _userProgramRepository.JoinAsync(userId, programId);

        if (joinProgram == null)
        {
            _logger.LogError("User is already joined to this program");
            throw new ArgumentException("User is already joined to this program");
        }
        
        return joinProgram.ToUserProgramDto();
    }

    public async Task LeaveAsync(Guid userId, Guid programId)
    {
        _logger.LogInformation("User {UserId} is leaving program {ProgramId}", userId, programId);
        await _userProgramRepository.LeaveAsync(userId, programId);
        
        _logger.LogInformation("User {UserId} successfully left program {ProgramId}", userId, programId);
    }

    public async Task<bool> IsJoinedAsync(Guid userId, Guid programId)
    {
        _logger.LogInformation("Checking if user {UserId} is joined to program {ProgramId}", userId, programId);
        var isJoined = await _userProgramRepository.IsJoinedAsync(userId, programId);
        
        _logger.LogInformation("User {UserId} is {Status}joined to program {ProgramId}", userId, isJoined ? "" : "not ", programId);
        return isJoined;
    }
    
    public async Task<List<ProgramDto>> GetProgramsForUser(Guid userId)
    {
        _logger.LogInformation("Fetching programs for user {UserId}", userId);
        var programsForUser = await _userProgramRepository.GetProgramsForUser(userId);
        
        var result = programsForUser.Select(p => p.ToProgramDto()).ToList();
        _logger.LogInformation("User {UserId} is enrolled in {Count} program(s)", userId, result.Count);
        
        return result;
    }
}

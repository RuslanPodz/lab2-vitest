using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Program;
using YogaApp.Core.Domain.User;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.User;

public class UserProgramRepository : IUserProgramRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserProgramRepository> _logger;

    public UserProgramRepository(AppDbContext context, ILogger<UserProgramRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProgramModel>> GetAllPrograms()
    {
        _logger.LogInformation("Retrieving all programs from the database.");
        var programs = await _context.Programs.ToListAsync();
        _logger.LogInformation("Retrieved {Count} programs.", programs.Count);
        return programs;
    }

    public async Task<UserProgramModel?> JoinAsync(Guid? userId, Guid programId)
    {
        _logger.LogInformation("User {UserId} attempting to join program {ProgramId}.", userId, programId);

        var existing = await _context.UserPrograms.FirstOrDefaultAsync(up => up.UserId == userId && up.ProgramId == programId);

        if (existing != null)
        {
            _logger.LogWarning("User {UserId} already joined program {ProgramId}.", userId, programId);
            return existing;
        }

        var newUserProgram = new UserProgramModel
        {
            UserProgramId = Guid.NewGuid(),
            ProgramId = programId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        };

        await _context.UserPrograms.AddAsync(newUserProgram);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {UserId} successfully joined program {ProgramId}.", userId, programId);
        return newUserProgram;
    }

    public async Task LeaveAsync(Guid? userId, Guid programId)
    {
        _logger.LogInformation("User {UserId} attempting to leave program {ProgramId}.", userId, programId);

        var existing = await _context.UserPrograms.FirstOrDefaultAsync(up => up.UserId == userId && up.ProgramId == programId);
        if (existing != null)
        {
            _context.UserPrograms.Remove(existing);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {UserId} left program {ProgramId}.", userId, programId);
        }
        else
        {
            _logger.LogWarning("User {UserId} tried to leave program {ProgramId}, but was not enrolled.", userId, programId);
        }
    }

    public async Task<bool> IsJoinedAsync(Guid userId, Guid programId)
    {
        _logger.LogInformation("Checking if user {UserId} is joined to program {ProgramId}.", userId, programId);

        var isJoined = await _context.UserPrograms.AnyAsync(up => up.UserId == userId && up.ProgramId == programId);

        _logger.LogInformation("User {UserId} is {Status}joined to program {ProgramId}.", userId, isJoined ? "" : "not ", programId);
        return isJoined;
    }

    public async Task<List<ProgramModel>> GetProgramsForUser(Guid? userId)
    {
        _logger.LogInformation("Getting programs joined by user {UserId}.", userId);

        var programsForUser = await _context.UserPrograms
            .Where(up => up.UserId == userId)
            .Include(up => up.Program)
            .Select(up => new ProgramModel
            {
                ProgramId = up.ProgramId,
                ProgramName = up.Program.ProgramName
            })
            .ToListAsync();

        _logger.LogInformation("User {UserId} is enrolled in {Count} programs.", userId, programsForUser.Count);
        return programsForUser;
    }
}

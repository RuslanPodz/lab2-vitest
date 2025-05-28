using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.Program;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Program;

public class ProgramRepository : IProgramRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProgramRepository> _logger;

    public ProgramRepository(AppDbContext context, ILogger<ProgramRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProgramModel>> GetAllPrograms()
    {
        _logger.LogInformation("Retrieving all programs.");
        var programs = await _context.Programs
            .AsNoTracking()
            .ToListAsync();
        _logger.LogInformation("Retrieved {Count} programs.", programs.Count);
        return programs;
    }

    public async Task<ProgramModel> GetProgramById(Guid id)
    {
        _logger.LogInformation("Retrieving program with ID: {ProgramId}", id);

        var program = await _context.Programs
            .Include(p => p.Workouts)
            .FirstOrDefaultAsync(p => p.ProgramId == id);

        if (program == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found.", id);
            throw new KeyNotFoundException($"Program with ID {id} not found.");
        }
        else
            _logger.LogInformation("Program with ID {ProgramId} retrieved successfully.", id);

        return program;
    }

    public async Task<List<ProgramModel>> GetProgramsByCategory(string categoryName)
    {
        _logger.LogInformation("Retrieving programs by category: {Category}", categoryName);
        var programs = await _context.Programs
            .Where(p => p.ProgramCategory == categoryName)
            .AsNoTracking()
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} programs in category '{Category}'.", programs.Count, categoryName);
        return programs;
    }

    public async Task<List<ProgramModel>> GetProgramsForUser(Guid userId)
    {
        var programsForUser = await _context.UserPrograms
            .Where(up => up.UserId == userId)
            .Include(up => up.Program) 
            .Select(up => new ProgramModel
            {
                ProgramId = up.ProgramId,
                ProgramName = up.Program.ProgramName
            })
            .AsNoTracking()
            .ToListAsync();

        return programsForUser;
    }

    public async Task<ProgramModel?> CreateProgram(ProgramModel program)
    {
        _logger.LogInformation("Creating new program: {ProgramName}", program.ProgramName);
        await _context.Programs.AddAsync(program);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Program created with ID: {ProgramId}", program.ProgramId);
        return program;
    }

    public async Task<ProgramModel> UpdateProgram(ProgramModel program)
    {
        _logger.LogInformation("Updating program with ID: {ProgramId}", program.ProgramId);
        var existingProgram = await _context.Programs.FindAsync(program.ProgramId);

        if (existingProgram == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found for update.", program.ProgramId);
            throw new KeyNotFoundException($"Program with ID {program.ProgramId} not found.");
        }

        _context.Entry(existingProgram).CurrentValues.SetValues(program);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Program with ID {ProgramId} updated successfully.", program.ProgramId);
        return existingProgram;
    }

    public async Task<ProgramModel> DeleteProgram(Guid id)
    {
        _logger.LogInformation("Deleting program with ID: {ProgramId}", id);
        var deletedProgram = await _context.Programs.FindAsync(id);

        if (deletedProgram == null)
        {
            _logger.LogWarning("Program with ID {ProgramId} not found for deletion.", id);
            throw new KeyNotFoundException($"Program with ID {id} not found.");
        }

        _context.Programs.Remove(deletedProgram);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Program with ID {ProgramId} deleted successfully.", id);
        return deletedProgram;
    }
}

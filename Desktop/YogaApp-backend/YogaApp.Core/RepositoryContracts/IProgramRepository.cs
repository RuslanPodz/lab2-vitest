using YogaApp.Core.Domain.Program;

namespace YogaApp.Core.RepositoryContracts;

public interface IProgramRepository
{
    Task<List<ProgramModel>> GetAllPrograms();
    Task<ProgramModel> GetProgramById(Guid id);
    Task<List<ProgramModel>> GetProgramsByCategory(string categoryName);
    Task<List<ProgramModel>> GetProgramsForUser(Guid userId);
    Task<ProgramModel?> CreateProgram(ProgramModel program);
    Task<ProgramModel> UpdateProgram(ProgramModel program);
    Task<ProgramModel> DeleteProgram(Guid id);
}
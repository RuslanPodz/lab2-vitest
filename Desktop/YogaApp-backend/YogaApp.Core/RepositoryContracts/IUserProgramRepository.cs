using YogaApp.Core.Domain.Program;
using YogaApp.Core.Domain.User;

namespace YogaApp.Core.RepositoryContracts;

public interface IUserProgramRepository
{
    Task<List<ProgramModel>> GetAllPrograms();
    Task<UserProgramModel?> JoinAsync(Guid? userId, Guid programId);
    Task LeaveAsync(Guid? userId, Guid programId);
    Task<bool> IsJoinedAsync(Guid userId, Guid programId);
    Task<List<ProgramModel>> GetProgramsForUser(Guid? userId);
}
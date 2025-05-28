using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos;

namespace YogaApp.Application.ServiceContact.User;

public interface IUserProgramService
{
    Task<UserProgramDto> JoinAsync(Guid userId, Guid programId);
    Task LeaveAsync(Guid userId, Guid programId);
    Task<bool> IsJoinedAsync(Guid userId, Guid programId);
    Task<List<ProgramDto>> GetProgramsForUser(Guid userId);
}
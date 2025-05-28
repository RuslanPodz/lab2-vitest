using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Core.Domain.User;

namespace YogaApp.Application.Mappers.User;

public static class UserProgramMapper
{
    public static UserProgramDto ToUserProgramDto(this UserProgramModel userProgram)
    {
        return new UserProgramDto()
        {
            UserProgramId = userProgram.UserProgramId,
            UserId = userProgram.UserId,
            ProgramId = userProgram.ProgramId,
            UserName = userProgram.User.Name,
            JoinedAt = userProgram.JoinedAt,
            CompletedAt = userProgram.CompletedAt,
        };
    }
}
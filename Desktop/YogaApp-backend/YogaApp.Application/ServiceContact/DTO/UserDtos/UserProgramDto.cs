using YogaApp.Application.ServiceContact.DTO.ProgramDtos;

namespace YogaApp.Application.ServiceContact.DTO.UserDtos;

public class UserProgramDto
{
    public Guid UserProgramId { get; set; } = Guid.NewGuid();
    
    public Guid? UserId { get; set; }
    public Guid ProgramId { get; set; }

    public DateTime JoinedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? UserName { get; set; }

    public ProgramDto Program { get; set; }
    public UserResponseDto User { get; set; }
}
namespace YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;

public class LoginResponseDto
{
    public string Message { get; set; } = "";
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

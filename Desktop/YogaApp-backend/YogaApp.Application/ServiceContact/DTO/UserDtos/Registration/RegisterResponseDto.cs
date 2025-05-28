namespace YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;

public class RegisterResponseDto
{
    public string Message { get; set; } = "Registered successfully";
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

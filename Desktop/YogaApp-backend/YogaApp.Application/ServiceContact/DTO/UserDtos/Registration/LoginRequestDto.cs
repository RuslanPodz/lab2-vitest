using System.ComponentModel.DataAnnotations;

namespace YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;

public class LoginRequestDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
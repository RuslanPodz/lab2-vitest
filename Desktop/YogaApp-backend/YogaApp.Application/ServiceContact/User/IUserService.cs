using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;

namespace YogaApp.Application.ServiceContact.User;

public interface IUserService
{
    Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto user);
    Task<LoginResponseDto> LoginUserAsync(LoginRequestDto user);
    Task UpdateUserAsync(Guid userId, UserRequestDto request);
    Task<UserResponseDto> GetUserByIdAsync(Guid userId);
    Task DeleteUserAsync(Guid userId);
}
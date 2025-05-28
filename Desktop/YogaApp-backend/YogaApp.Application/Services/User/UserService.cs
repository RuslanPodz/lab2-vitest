using Microsoft.AspNetCore.Identity;
using YogaApp.Application.Helpers;
using YogaApp.Application.Mappers.User;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;
using YogaApp.Application.ServiceContact.User;
using YogaApp.Core.Domain.User;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository,
        JwtTokenHelper jwtTokenHelper,
        ILogger<UserService> logger)
    {
        _jwtTokenHelper = jwtTokenHelper;
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task<RegisterResponseDto> RegisterUserAsync(RegisterRequestDto user)
{
    _logger.LogInformation("Attempting to register user with email: {Email}", user.Email);
    
    var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
    if (existingUser != null)
    {
        _logger.LogWarning("Registration failed: user with email {Email} already exists", user.Email);
        throw new Exception("User with this email already exists.");
    }

    var hasher = new PasswordHasher<UserModel>();
    var userModel = user.ToUserModel(hasher);
    await _userRepository.AddUserAsync(userModel);

    _logger.LogInformation("User registered successfully with ID: {UserId}", userModel.UserId);

    return new RegisterResponseDto
    {
        Message = "Registration successful",
        UserId = userModel.UserId,
        Name = userModel.Name,
        Email = userModel.Email
    };
}

public async Task<LoginResponseDto> LoginUserAsync(LoginRequestDto user)
{
    _logger.LogInformation("Attempting login for email: {Email}", user.Email);

    var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
    if (existingUser == null)
    {
        _logger.LogWarning("Login failed: user with email {Email} not found", user.Email);
        throw new Exception("Invalid email or password.");
    }

    var hasher = new PasswordHasher<UserModel>();
    var result = hasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, user.Password);

    if (result == PasswordVerificationResult.Failed)
    {
        _logger.LogWarning("Login failed: invalid password for email {Email}", user.Email);
        throw new Exception("Invalid email or password.");
    }

    var token = _jwtTokenHelper.GenerateJwtToken(existingUser);

    _logger.LogInformation("Login successful for user {UserId}", existingUser.UserId);

    return new LoginResponseDto
    {
        UserId = existingUser.UserId,
        Token = token,
        Name = existingUser.Name,
        Email = existingUser.Email,
        Message = "Login successful"
    };
}

public async Task UpdateUserAsync(Guid userId, UserRequestDto request)
{
    _logger.LogInformation("Attempting to update user with ID: {UserId}", userId);

    var existingUser = await _userRepository.GetUserByIdAsync(userId);
    if (existingUser == null)
    {
        _logger.LogWarning("Update failed: user with ID {UserId} not found", userId);
        throw new Exception("User not found");
    }

    existingUser.Name = request.Name;
    existingUser.Email = request.Email;
    existingUser.PasswordHash = request.Password;

    await _userRepository.UpdateUserAsync(existingUser);

    _logger.LogInformation("User with ID {UserId} successfully updated", userId);
}

public async Task<UserResponseDto> GetUserByIdAsync(Guid userId)
{
    _logger.LogInformation("Fetching user by ID: {UserId}", userId);

    var user = await _userRepository.GetUserByIdAsync(userId);
    if (user == null)
    {
        _logger.LogWarning("User not found for ID: {UserId}", userId);
        throw new Exception("User not found");
    }

    _logger.LogInformation("User fetched successfully for ID: {UserId}", userId);

    return user.ToUserResponseDto();
}

public async Task DeleteUserAsync(Guid userId)
{
    _logger.LogInformation("Deleting user with ID: {UserId}", userId);
    await _userRepository.DeleteUserAsync(userId);
    _logger.LogInformation("User with ID {UserId} deleted successfully", userId);
}

}
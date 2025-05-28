using Microsoft.AspNetCore.Identity;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;
using YogaApp.Core.Domain.User;

namespace YogaApp.Application.Mappers.User;

public static class UserMapper
{
    public static UserModel ToUserRequestDto(this UserModel user) => new()
    {
        UserId = Guid.NewGuid(),
        Name = user.Name,
        Email = user.Email,
        PasswordHash = user.PasswordHash
    };
    
    public static UserResponseDto ToUserResponseDto(this UserModel user) => new()
    {
        UserId = user.UserId,
        Name = user.Name,
        Email = user.Email,
        Password = user.PasswordHash,
        
    };
    
    public static UserModel ToUserModel(this RegisterRequestDto dto, IPasswordHasher<UserModel> hasher)
    {
        var user = new UserModel
        {
            UserId = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
        };
        user.PasswordHash = hasher.HashPassword(user, dto.Password);
        return user;
    }
    
    
}
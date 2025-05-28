using YogaApp.Core.Domain.User;

namespace YogaApp.Core.RepositoryContracts;

public interface IUserRepository
{
    Task<UserModel?> GetUserByEmailAsync(string email);
    Task<UserModel> AddUserAsync(UserModel user);
    Task<UserModel> GetUserByIdAsync(Guid userId);
    Task<UserModel> UpdateUserAsync(UserModel user);
    Task DeleteUserAsync(Guid userId);
}
using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.User;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.User;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation("Searching for user with email: {Email}", email);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            _logger.LogWarning("No user found with email: {Email}", email);
        else
            _logger.LogInformation("User with email {Email} found: {UserId}", email, user.UserId);

        return user;
    }

    public async Task<UserModel> AddUserAsync(UserModel user)
    {
        if (user == null)
        {
            _logger.LogError("Attempted to add null user.");
            throw new ArgumentNullException(nameof(user));
        }

        _logger.LogInformation("Attempting to add user with email: {Email}", user.Email);
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists. Returning existing user.", user.Email);
            return existingUser;
        }

        var newUser = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("New user created with ID: {UserId}", newUser.Entity.UserId);
        return newUser.Entity;
    }

    public async Task<UserModel> GetUserByIdAsync(Guid userId)
    {
        _logger.LogInformation("Fetching user by ID: {UserId}", userId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
        {
            _logger.LogError("User with ID {UserId} not found.", userId);
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        return user;
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user)
    {
        if (user == null)
        {
            _logger.LogError("Attempted to update null user.");
            throw new ArgumentNullException(nameof(user));
        }

        _logger.LogInformation("Updating user with ID: {UserId}", user.UserId);
        var updatedUser = _context.Users.Update(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User with ID {UserId} updated successfully.", user.UserId);
        return updatedUser.Entity;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        _logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);
        var user = await GetUserByIdAsync(userId);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User with ID {UserId} deleted.", userId);
    }
}

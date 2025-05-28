using Microsoft.EntityFrameworkCore;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.RepositoryContracts;
using YogaApp.Infrastructure.Data;

namespace YogaApp.Infrastructure.Repository.Progress;

public class UserWeightTrackingRepository : IUserWeightTrackingRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserWeightTrackingRepository> _logger;

    public UserWeightTrackingRepository(AppDbContext context, ILogger<UserWeightTrackingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserWeightTrackingModel?> GetWeightStatsAsync(Guid? userId)
    {
        _logger.LogInformation("Retrieving weight stats for user ID: {UserId}", userId);
        var weightStats = await _context.UserWeightTrackings.FirstOrDefaultAsync(uwt => uwt.UserId == userId);
        if (weightStats == null)
            _logger.LogWarning("No weight stats found for user ID: {UserId}", userId);
        return weightStats;
    }

    public async Task<UserWeightTrackingModel?> GetByUserIdAsync(Guid? userId)
    {
        _logger.LogInformation("Retrieving weight tracking record for user ID: {UserId}", userId);
        var getUser = await _context.UserWeightTrackings.FirstOrDefaultAsync(uwt => uwt.UserId == userId);
        if (getUser == null)
            _logger.LogWarning("No tracking record found for user ID: {UserId}", userId);
        return getUser;
    }

    public async Task AddAsync(UserWeightTrackingModel tracking)
    {
        _logger.LogInformation("Adding weight tracking for user ID: {UserId}", tracking.UserId);
        await _context.UserWeightTrackings.AddAsync(tracking);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Weight tracking added for user ID: {UserId}", tracking.UserId);
    }

    public async Task UpdateAsync(UserWeightTrackingModel tracking)
    {
        _logger.LogInformation("Updating weight tracking for user ID: {UserId}", tracking.UserId);
        var existingTracking = await GetByUserIdAsync(tracking.UserId);

        if (existingTracking != null)
        {
            _logger.LogInformation("Existing tracking found, updating values for user ID: {UserId}", tracking.UserId);

            existingTracking.StartWeight = tracking.StartWeight;
            existingTracking.CurrentWeight = tracking.CurrentWeight;
            existingTracking.Height = tracking.Height;
            existingTracking.BMIStatus = tracking.BMIStatus;

            _context.Entry(existingTracking).CurrentValues.SetValues(existingTracking);
        }
        else
        {
            _logger.LogInformation("No tracking found, creating new for user ID: {UserId}", tracking.UserId);
            await _context.UserWeightTrackings.AddAsync(tracking);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Weight tracking saved for user ID: {UserId}", tracking.UserId);
    }
}

using YogaApp.Application.Helpers;
using YogaApp.Application.Mappers.Progress;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;
using YogaApp.Application.ServiceContact.Progress;
using YogaApp.Core.Domain.User.UserProgress;
using YogaApp.Core.RepositoryContracts;

namespace YogaApp.Application.Services.Progress;

public class UserWeightTrackingService : IUserWeightTrackingService
{
    private readonly IUserWeightTrackingRepository _userWeightTrackingRepository;
    private readonly ILogger<UserWeightTrackingService> _logger;
    
    public UserWeightTrackingService(IUserWeightTrackingRepository userWeightTrackingRepository,
        ILogger<UserWeightTrackingService> logger)
    {
        _logger = logger;
        _userWeightTrackingRepository = userWeightTrackingRepository;
    }
    public async Task<UserWeightTrackingDto> GetWeightStatsAsync(Guid? userId)
    {
        _logger.LogInformation("Getting weight stats for user with ID: {UserId}", userId);
        var weightStats = await _userWeightTrackingRepository.GetWeightStatsAsync(userId);
        
        if (weightStats == null)
        {
            _logger.LogWarning("No weight stats found for user with ID: {UserId}", userId);
            throw new Exception("No weight stats found");
        }
        return weightStats.ToUserWeightTrackingDto();
    }

    public async Task AddUserInitialWeightAsync(Guid? userId, UserWeightTrackingInitialDataDto request)
    {
        if (userId == null)
        {
            _logger.LogError("User ID is null");
            throw new ArgumentNullException(nameof(userId));
        }

        _logger.LogInformation("Adding initial weight for user with ID: {UserId}", userId);
        var newEntry = new UserWeightTrackingModel()
        {
            WeightTrackingId = Guid.NewGuid(),
            UserId = userId.Value,
            StartWeight = request.StartWeight,
            Height = request.Height,
        };

        await _userWeightTrackingRepository.AddAsync(newEntry);
    }
    
    public async Task<double> UpdateWeightAndCalculateBMIAsync(Guid? userId, double newWeight, double heightInMeters)
    {
        _logger.LogInformation("Updating weight for user {UserId}. New weight: {Weight}kg, Height: {Height}m", userId, newWeight, heightInMeters);

        var bmi = BMICalculator.CalculateBMI(newWeight, heightInMeters);
        var bmiStatus = BMICalculator.CalculateBmiStatus(bmi);

        _logger.LogInformation("Calculated BMI: {BMI}, Status: {Status}", bmi, bmiStatus);

        var tracking = await _userWeightTrackingRepository.GetByUserIdAsync(userId);

        if (tracking == null)
        {
            _logger.LogInformation("No existing weight tracking found for user {UserId}. Creating new record.", userId);

            tracking = new UserWeightTrackingModel
            {
                WeightTrackingId = Guid.NewGuid(),
                UserId = userId,
                StartWeight = newWeight,
                CurrentWeight = newWeight,
                Height = heightInMeters,
                BMI = bmi,
                BMIStatus = bmiStatus,
                DifferenceInWeight = 0
            };

            await _userWeightTrackingRepository.AddAsync(tracking);
            _logger.LogInformation("New weight tracking record added for user {UserId}", userId);
        }
        else
        {
            _logger.LogInformation("Updating existing weight tracking record for user {UserId}", userId);

            tracking.CurrentWeight = newWeight;
            tracking.Height = heightInMeters;
            tracking.DifferenceInWeight = newWeight - tracking.StartWeight;
            tracking.BMI = bmi;
            tracking.BMIStatus = bmiStatus;

            await _userWeightTrackingRepository.UpdateAsync(tracking);
            _logger.LogInformation("Weight tracking record updated for user {UserId}", userId);
        }

        return bmi;
    }

}
using Microsoft.AspNetCore.Mvc;
using MobileYogaApp.Extensions;
using MobileYogaApp.Presentation.ViewModels.ProgressPageModel;
using YogaApp.Application.ServiceContact.DTO.UserDtos;
using YogaApp.Application.ServiceContact.DTO.UserDtos.WeightTracking;
using YogaApp.Application.ServiceContact.Progress;

namespace MobileYogaApp.Controllers;

[ApiController]
[Route("progress")]
public class ProgressPageController : ControllerBase
{
    private readonly IUserGeneralStatsService _userGeneralStatsService;
    private readonly IUserWeightTrackingService _userWeightTrackingService;
    private readonly ILogger<ProgressPageController> _logger;
    
    public ProgressPageController(IUserGeneralStatsService userGeneralStatsService, 
        IUserWeightTrackingService userWeightTrackingService,
        ILogger<ProgressPageController> logger)
    {
        _userGeneralStatsService = userGeneralStatsService;
        _userWeightTrackingService = userWeightTrackingService;
        _logger = logger;
    }
    
    [HttpGet("weight-tracking")]
    public async Task<IActionResult> GetWeightTracking()
    {
        Guid? userId = User.GetUserId();
        _logger.LogInformation("User {UserId} is requesting weight tracking", userId);
        
        var weightTracking = new WeightTracking()
        {
            Title = "Weight",
            UserWeight = await _userWeightTrackingService.GetWeightStatsAsync(userId)
        };
        
        if (weightTracking.UserWeight == null)
        {
            _logger.LogWarning("User weight not found");
            return NotFound("User stats not found.");
        }
        
        return Ok(weightTracking);
    }
    
    [HttpGet("general-stats")]
    public async Task<IActionResult> GetGeneralStats()
    {
        _logger.LogInformation("Get user general stats");
        var userId = User.GetUserId();
        
        var generalStatsPage = new GeneralStats()
        {
            Title = "Total",
            Stats = await _userGeneralStatsService.GetGeneralStats(userId)
        };
    
        if (generalStatsPage.Stats == null)
        {
            _logger.LogWarning("User general stats not found");
            return NotFound("User stats not found.");
        }

        return Ok(generalStatsPage);
    }

    [HttpPost("add-entry-weight")]
    public async Task<IActionResult> AddEntryWeight([FromBody] UserWeightTrackingInitialDataDto request)
    {
        var userId = User.GetUserId();
        try
        {
            await _userWeightTrackingService.AddUserInitialWeightAsync(userId, request);
            _logger.LogInformation("Користувач {UserId} успішно додав початкову вагу", userId);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при додаванні ваги користувачем {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }

    }

    [HttpPut("calculate-bmi")]
    public async Task<IActionResult> UpdateWeightAndCalculateBmiAsync([FromBody] UserCalculateBmiRequest request)
    {
        _logger.LogInformation("Update user weight and calculate BMI");
        var userId = User.GetUserId();
        
        await _userWeightTrackingService.UpdateWeightAndCalculateBMIAsync(userId, request.Weight, request.HeightInMeters);
        _logger.LogInformation("UpdateWeightAndCalculateBmi: Weight: {Weight}, Height: {Height}", request.Weight, request.HeightInMeters);
        return Ok();
    }
    
}
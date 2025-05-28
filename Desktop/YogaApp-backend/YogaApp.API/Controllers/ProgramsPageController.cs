using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileYogaApp.Extensions;
using MobileYogaApp.Presentation.ViewModels;
using YogaApp.Application.ServiceContact.DTO.ProgramDtos;
using YogaApp.Application.ServiceContact.Program;

namespace MobileYogaApp.Controllers;

[Authorize]
[ApiController]
[Route("programs")]
public class ProgramsPageController : ControllerBase
{
    private readonly IProgramService _programService;
    private readonly ILogger<ProgramsPageController> _logger;
    
    public ProgramsPageController(IProgramService programService,
        ILogger<ProgramsPageController> logger)
    {
        _logger = logger;
        _programService = programService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPrograms()
    {
        _logger.LogInformation("Get all programs");
        
        var programPageModel = new ProgramPageModel()
        {
            Title = "Programs",
            Programs = await _programService.GetAllProgramsPreviewAsync()
        };
        return Ok(programPageModel);
    }

    [HttpGet("/{programId}")]
    public async Task<IActionResult> GetProgramByIdAsync(Guid programId)
    {
        _logger.LogInformation("Get program by id: {programId}", programId);
        var program = await _programService.GetProgramByIdAsync(programId);
        return Ok(program);
    }
    
    [HttpGet("{programId}/workouts")]
    public async Task<IActionResult> GetProgramWorkoutsAsync(Guid programId)
    {
        _logger.LogInformation("Get program workouts by id: {programId}", programId);
        var workouts = await _programService.GetProgramWorkoutsAsync(programId);
        return Ok(workouts);
    }
    
    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetProgramsByCategoryAsync(string categoryName)
    {
        _logger.LogInformation("Get programs by category: {categoryName}", categoryName);
        var programs = await _programService.GetProgramsByCategoryAsync(categoryName);
        return Ok(programs);
    }
    
    [HttpGet("user-programs")]
    public async Task<IActionResult> GetProgramsForUser()
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Get programs for user with id: {userId}", userId);
        var programs = await _programService.GetProgramsForUserAsync(userId);
        return Ok(programs);
    }
    
    [HttpPost("add-new-program")]
    public async Task<IActionResult> CreateProgram([FromBody] ProgramRequestDto program)
    {
        _logger.LogInformation("Create new program: {programName}", program.Name);
        var createdProgram = await _programService.CreateProgramAsync(program);
        return Ok(new { createdProgram.ProgramId });
    }
    
    [HttpPost("{programId}/add-workout")]
    public async Task<IActionResult> AddWorkoutToProgram(Guid programId, [FromBody] AddWorkoutToProgramRequest request)
    {
        _logger.LogInformation("Add workout to program with id: {programId}", programId);
        var success = request.Workout != null && await _programService.AddWorkoutToProgramAsync(programId, request.Workout, request.ExistingWorkoutName);

        if (!success)
        {
            _logger.LogWarning("Failed to add workout to program with id: {programId}", programId);
            return BadRequest("Failed to add workout");
        }

        return Ok("Workout added to program");
    }

    [HttpPost("join-program/{programId}")]
    public async Task<IActionResult> JoinProgram(Guid programId)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Join program with id: {programId}", programId);
        await _programService.JoinProgramAsync(userId, programId);
        return Ok();
    }
    
    [HttpPost("leave-program/{programId}")]
    public async Task<IActionResult> LeaveProgram(Guid programId)
    {
        var userId = User.GetUserId();
        _logger.LogInformation("Leave program with id: {programId}", programId);
        await _programService.LeaveProgramAsync(userId, programId);
        return Ok();
    }
    
    [HttpPut("update-program")]
    public async Task<IActionResult> UpdateProgram([FromBody] ProgramRequestDto program)
    {
        _logger.LogInformation("Update program with name: {programName}",program.Name);
        await _programService.UpdateProgramAsync(program);
        return Ok();
    }
    
    [HttpDelete("delete-program/{programId}")]
    public async Task<IActionResult> DeleteProgram(Guid programId)
    {
        _logger.LogInformation("Delete program with id: {programId}", programId);
        await _programService.DeleteProgramAsync(programId);
        return Ok();
    }
}
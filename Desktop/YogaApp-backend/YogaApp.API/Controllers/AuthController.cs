using Microsoft.AspNetCore.Mvc;
using YogaApp.Application.ServiceContact.DTO.UserDtos.Registration;
using YogaApp.Application.ServiceContact.User;

namespace MobileYogaApp.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", request.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid registration model for email: {Email}", request.Email);
            return BadRequest(ModelState);
        }

        var user = await _userService.RegisterUserAsync(request);

        _logger.LogInformation("User registered successfully: {UserId}", user.UserId);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for email: {Email}", request.Email);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid login model for email: {Email}", request.Email);
            return BadRequest(ModelState);
        }

        var loginPerson = await _userService.LoginUserAsync(request);

        HttpContext.Response.Cookies.Append("jwt", loginPerson.Token);

        _logger.LogInformation("User logged in successfully: {UserId}", loginPerson.UserId);
        return Ok(loginPerson);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Response.Cookies.Delete("jwt");
        _logger.LogInformation("User logged out");

        return Ok(new { message = "Logout successful" });
    }
}

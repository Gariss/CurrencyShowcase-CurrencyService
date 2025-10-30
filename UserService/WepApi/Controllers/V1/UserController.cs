using UserService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserService.Application.Contracts.Responses;

namespace UserService.WebApi.Controllers.V1;

public class UserController(
    IUserService userService,
    ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    /// <summary>
    /// Register new user
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.RegisterAsync(request, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "User registration error");
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User registration error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "User login error");
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User login error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        try
        {
            var userLogin = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _userService.LogoutAsync(userLogin, cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User logout error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromQuery] string login, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.GetByLoginAsync(login, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User profile loading error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }
}
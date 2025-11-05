using UserService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Contracts.Requests;
using UserService.Application.Contracts.Responses;

namespace UserService.WebApi.Controllers.V1;

public class UsersController(
    IUserService userService,
    ILogger<UsersController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UsersController> _logger = logger;

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.LoginAsync(request, cancellationToken);
            Response.Headers.Append("Authorization", $"Bearer {result.Token}");

            return Ok();
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

    /// <summary>
    /// Get user profile
    /// </summary>
    [HttpGet]
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
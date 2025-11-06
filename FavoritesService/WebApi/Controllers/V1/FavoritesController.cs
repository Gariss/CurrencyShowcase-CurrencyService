using Microsoft.AspNetCore.Mvc;
using FavoritesService.Application.Services;
using FavoritesService.Domain.Entities;

namespace FavoritesService.WebApi.Controllers.V1;

public class FavoritesController(
    IFavoritesService favoritesService,
    ILogger<FavoritesController> logger) : ControllerBase
{
    private readonly IFavoritesService _favoritesService = favoritesService;
    private readonly ILogger<FavoritesController> _logger = logger;

    [HttpGet]
    [ProducesResponseType<List<Currency>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFavorites(CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserIdFromContext();
            var favorites = await _favoritesService.GetUserFavoritesAsync(userId, cancellationToken);

            return Ok(favorites);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "User identification error");
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User favorites loading error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddToFavorites([FromBody] string[] currencyIds, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserIdFromContext();
            await _favoritesService.AddToFavoritesAsync(userId, currencyIds, cancellationToken);

            return Ok();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "User identification error");
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User favorites saving error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveFromFavorites([FromBody] string[] currencyIds, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserIdFromContext();
            await _favoritesService.RemoveFromFavoritesAsync(userId, currencyIds, cancellationToken);

            return Ok();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "User identification error");
            return Problem(statusCode: StatusCodes.Status401Unauthorized, title: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "User favorites removing error");
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "Internal error");
        }
    }

    private Guid GetUserIdFromContext()
    {
        var userIdString = HttpContext.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
        {
            throw new UnauthorizedAccessException("User is not identified"); 
        }

        return userId;
    }
}
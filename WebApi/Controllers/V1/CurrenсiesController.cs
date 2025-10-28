using CurrencyUpdaterService.Application.Services;
using CurrencyUpdaterService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyUpdaterService.WebApi.Controllers.V1;

public class CurrenсiesController(
    ICurrencyService currencyService) : ControllerBase
{
    private readonly ICurrencyService _currencyService = currencyService;

    /// <summary>
    /// Get list of all currencies
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<Currency>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var result = await _currencyService.GetListAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get currency by name (char code)
    /// </summary>
    [HttpGet("{name}")]
    [ProducesResponseType(typeof(Currency), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNameAsync(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        var result = await _currencyService.GetByNameAsync(name, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Refresh currency rates
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshCurrenciesAsync(CancellationToken cancellationToken)
    {
        var result = await _currencyService.RefreshCurrenciesAsync(cancellationToken);

        if (!result)
        {
            return Problem("Something went wrong", statusCode: 500);
        }

        return Ok();
    }
}
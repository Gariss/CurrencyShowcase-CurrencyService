using CurrencyService.Application.Services;
using CurrencyService.Domain.Entities;
using CurrencyService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.WebApi.Controllers.V1;

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
    /// Get currency by char code
    /// </summary>
    [HttpGet("{charCode}")]
    [ProducesResponseType(typeof(Currency), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByNameAsync(
        [FromRoute] string charCode,
        CancellationToken cancellationToken)
    {
        var result = await _currencyService.GetByCharCodeAsync(charCode, cancellationToken);

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
            return Problem("Unable to refresh currencies", statusCode: 500);
        }

        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
}
using Microsoft.AspNetCore.Mvc;

namespace CurrencyUpdaterService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
}
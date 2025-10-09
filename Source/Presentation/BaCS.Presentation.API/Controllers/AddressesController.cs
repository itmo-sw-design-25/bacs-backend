namespace BaCS.Presentation.API.Controllers;

using Application.Abstractions.Integrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
[ApiController]
[Route("addresses")]
public class AddressesController(IAddressSuggestionsService addressSuggestions) : ControllerBase
{
    [EndpointSummary("Получить подсказки адреса по подстроке.")]
    [HttpPost("suggest")]
    [ProducesResponseType<string[]>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IActionResult> SuggestAddress(
        [FromQuery] string query,
        [FromQuery] int count = 5,
        CancellationToken cancellationToken = default
    )
    {
        var result = await addressSuggestions.SuggestAddresses(query, count, cancellationToken);

        return Ok(result);
    }
}

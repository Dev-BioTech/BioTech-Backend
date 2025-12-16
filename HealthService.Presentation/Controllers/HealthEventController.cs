using MediatR;
using Microsoft.AspNetCore.Mvc;
using HealthService.Application.Commands.CreateHealthEvent;
using HealthService.Application.DTOs;

namespace HealthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthEventController : ControllerBase
{
    private readonly ISender _sender;

    public HealthEventController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<HealthEventResponse>> CreateHealthEvent([FromBody] CreateHealthEventCommand command)
    {
        var response = await _sender.Send(command);
        return CreatedAtAction(nameof(CreateHealthEvent), new { id = response.Id }, response);
    }
}

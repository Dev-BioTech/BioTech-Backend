using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReproductionService.Application.Commands.CreateReproductionEvent;
using ReproductionService.Application.DTOs;

namespace ReproductionService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReproductionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReproductionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ReproductionEventResponse>> Create(CreateReproductionEventCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}

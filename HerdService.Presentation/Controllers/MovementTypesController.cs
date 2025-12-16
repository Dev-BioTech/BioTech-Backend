using HerdService.Application.Commands.CreateMovementType;
using HerdService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HerdService.Presentation.Controllers;

[ApiController]
[Route("api/v1/movement-types")]
public class MovementTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovementTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<MovementTypeResponse>> Create(CreateMovementTypeCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}

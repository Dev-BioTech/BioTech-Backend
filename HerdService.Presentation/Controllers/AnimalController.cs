using HerdService.Application.Commands.CreateAnimal;
using HerdService.Application.Commands.RegisterAnimalMovement;
using HerdService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HerdService.Presentation.Controllers;

[ApiController]
[Route("api/v1/animals")]
public class AnimalController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnimalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<AnimalResponse>> Create(CreateAnimalCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    [HttpPost("{id}/movements")]
    public async Task<ActionResult<AnimalMovementResponse>> RegisterMovement(long id, RegisterAnimalMovementCommand command)
    {
        if (id != command.AnimalId)
        {
            return BadRequest("Animal ID mismatch");
        }
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(RegisterMovement), new { id = result.Id }, result);
    }
}

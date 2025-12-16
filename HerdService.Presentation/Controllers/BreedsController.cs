using HerdService.Application.Commands.CreateBreed;
using HerdService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HerdService.Presentation.Controllers;

[ApiController]
[Route("api/v1/breeds")]
public class BreedsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BreedsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BreedResponse>> Create(CreateBreedCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}

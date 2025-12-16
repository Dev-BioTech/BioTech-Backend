using HerdService.Application.Commands.CreatePaddock;
using HerdService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HerdService.Presentation.Controllers;

[ApiController]
[Route("api/v1/paddocks")]
public class PaddocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaddocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PaddockResponse>> Create(CreatePaddockCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}

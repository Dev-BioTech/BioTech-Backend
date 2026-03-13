using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Queries.GetMovementTypes;
using HerdService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/movement-types")]
public class MovementTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovementTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MovementTypeResponse>>>> GetAll()
    {
        var result = await _mediator.Send(new GetMovementTypesQuery());
        return Ok(ApiResponse<IEnumerable<MovementTypeResponse>>.Ok(result));
    }
}

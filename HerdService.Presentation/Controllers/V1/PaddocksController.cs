using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Queries.GetPaddocksByFarm;
using HerdService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/paddocks")]
public class PaddocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaddocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PaddockResponse>>>> GetByFarm([FromQuery] int farmId)
    {
        var result = await _mediator.Send(new GetPaddocksByFarmQuery(farmId));
        return Ok(ApiResponse<IEnumerable<PaddockResponse>>.Ok(result));
    }
}

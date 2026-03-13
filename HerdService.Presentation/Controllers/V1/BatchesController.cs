using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Queries.GetBatchesByFarm;
using HerdService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/batches")]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BatchResponse>>>> GetByFarm([FromQuery] int farmId)
    {
        var result = await _mediator.Send(new GetBatchesByFarmQuery(farmId));
        return Ok(ApiResponse<IEnumerable<BatchResponse>>.Ok(result));
    }
}

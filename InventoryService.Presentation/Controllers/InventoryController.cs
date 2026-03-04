using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using System.Collections.Generic;
using Shared.Infrastructure.Common;

namespace InventoryService.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> Create([FromBody] CreateInventoryItemCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByFarm), new { farmId = result.FarmId }, ApiResponse<InventoryItemDto>.Ok(result, "Inventory item created successfully"));
    }

    [HttpGet("farm/{farmId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<InventoryItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<InventoryItemDto>>>> GetByFarm(int farmId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetInventoryItemsQuery(farmId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<InventoryItemDto>>.Ok(result));
    }
}

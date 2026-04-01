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
    private readonly InventoryService.Presentation.Services.GatewayAuthenticationService _authService;

    public InventoryController(IMediator mediator, InventoryService.Presentation.Services.GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InventoryItemDto>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<InventoryItemDto>>> Create([FromBody] CreateInventoryItemCommand command)
    {
        var farmId = _authService.GetFarmId();
        if (!farmId.HasValue) return BadRequest(ApiResponse<InventoryItemDto>.Fail("User is not associated with a valid Farm"));

        var secureCommand = command with { FarmId = farmId.Value };
        var result = await _mediator.Send(secureCommand);
        return CreatedAtAction(nameof(GetByFarm), new { farmId = result.FarmId }, ApiResponse<InventoryItemDto>.Ok(result, "Inventory item created successfully"));
    }

    [HttpGet("farm/{farmId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<InventoryItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<InventoryItemDto>>>> GetByFarm(int farmId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var authFarmId = _authService.GetFarmId();
        if (authFarmId.HasValue && authFarmId.Value != farmId)
            return Unauthorized(ApiResponse<IEnumerable<InventoryItemDto>>.Fail("Access mismatch for Farm ID"));

        var query = new GetInventoryItemsQuery(farmId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<InventoryItemDto>>.Ok(result));
    }
}

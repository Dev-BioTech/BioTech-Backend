using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory-movements")]
public class InventoryMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> RegisterMovement(RegisterMovementDto dto)
    {
        try
        {
            var id = await _mediator.Send(new RegisterMovementCommand(dto));
            return Ok(ApiResponse<long>.Ok(id, "Movement registered successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<long>.Fail(ex.Message));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<long>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<long>.Fail("An error occurred while processing your request"));
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InventoryMovementDto>>>> GetKardex(int productId)
    {
        var movements = await _mediator.Send(new GetMovementsByProductQuery(productId));
        return Ok(ApiResponse<IEnumerable<InventoryMovementDto>>.Ok(movements));
    }
}

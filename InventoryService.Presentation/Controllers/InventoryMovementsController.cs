using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> RegisterMovement(RegisterMovementDto dto)
    {
        try
        {
            var id = await _mediator.Send(new RegisterMovementCommand(dto));
            return Ok(id);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<InventoryMovementDto>>> GetKardex(int productId)
    {
        var movements = await _mediator.Send(new GetMovementsByProductQuery(productId));
        return Ok(movements);
    }
}

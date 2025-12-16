using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Application.Queries;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] // Uncomment if auth is needed, usually yes for this service
public class FarmController : ControllerBase
{
    private readonly IMediator _mediator;

    public FarmController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<FarmResponseDto>> Create([FromBody] CreateFarmDto createFarmDto)
    {
        var command = new CreateFarmCommand(createFarmDto);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FarmResponseDto>> GetById(int id)
    {
        var query = new GetFarmByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<ActionResult<IEnumerable<FarmResponseDto>>> GetAllByTenant(int tenantId)
    {
        var query = new GetFarmsByTenantIdQuery(tenantId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteFarmCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}

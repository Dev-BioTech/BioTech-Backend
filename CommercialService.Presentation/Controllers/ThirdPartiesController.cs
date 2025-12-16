using CommercialService.Application.Commands;
using CommercialService.Application.DTOs;
using CommercialService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialService.Presentation.Controllers;

[ApiController]
[Route("api/third-parties")]
[Authorize]
public class ThirdPartiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ThirdPartiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> Create([FromBody] CreateThirdPartyDto dto)
    {
        try
        {
            var id = await _mediator.Send(new CreateThirdPartyCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = id }, id);
        }
        catch (System.ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(long id, [FromBody] UpdateThirdPartyDto dto)
    {
        var result = await _mediator.Send(new UpdateThirdPartyCommand(id, dto));
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<ThirdPartyDto>>> GetAll(
        [FromQuery] int farmId,
        [FromQuery] bool? isSupplier,
        [FromQuery] bool? isCustomer)
    {
        var result = await _mediator.Send(new GetThirdPartiesQuery(farmId, isSupplier, isCustomer));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ThirdPartyDto>> GetById(long id)
    {
        var result = await _mediator.Send(new GetThirdPartyByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands;
using SalesService.Application.Queries;
using SalesService.Application.DTOs;
using FluentValidation;

namespace SalesService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetSales()
    {
        try
        {
            var result = await _mediator.Send(new GetSalesByUserQuery());
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetSaleByIdQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale([FromBody] CreateSaleDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateSaleCommand(dto));
            return CreatedAtAction(nameof(GetSale), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SaleDto>> UpdateSale(int id, [FromBody] UpdateSaleDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateSaleCommand(id, dto));
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSale(int id)
    {
        try
        {
            await _mediator.Send(new DeleteSaleCommand(id));
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesService.Application.Commands;
using SalesService.Application.Queries;
using SalesService.Application.DTOs;
using FluentValidation;
using Shared.Infrastructure.Common;

namespace SalesService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/sales")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SaleDto>>>> GetSales()
    {
        try
        {
            var result = await _mediator.Send(new GetSalesByUserQuery());
            return Ok(ApiResponse<IEnumerable<SaleDto>>.Ok(result, "Sales retrieved successfully"));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(ApiResponse<IEnumerable<SaleDto>>.Fail("Unauthorized access"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<SaleDto>>.Fail("Internal server error"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SaleDto>>> GetSale(int id)
    {
        try
        {
            var result = await _mediator.Send(new GetSaleByIdQuery(id));
            if (result == null)
                return NotFound(ApiResponse<SaleDto>.Fail("Sale not found"));
            return Ok(ApiResponse<SaleDto>.Ok(result, "Sale retrieved successfully"));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(ApiResponse<SaleDto>.Fail("Unauthorized access"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<SaleDto>.Fail("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SaleDto>>> CreateSale([FromBody] CreateSaleDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateSaleCommand(dto));
            return CreatedAtAction(nameof(GetSale), new { id = result.Id }, ApiResponse<SaleDto>.Ok(result, "Sale created successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<SaleDto>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<SaleDto>.Fail(ex.Message));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(ApiResponse<SaleDto>.Fail("Unauthorized access"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<SaleDto>.Fail("Internal server error"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SaleDto>>> UpdateSale(int id, [FromBody] UpdateSaleDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateSaleCommand(id, dto));
            return Ok(ApiResponse<SaleDto>.Ok(result, "Sale updated successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<SaleDto>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<SaleDto>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<SaleDto>.Fail("Internal server error"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteSale(int id)
    {
        try
        {
            await _mediator.Send(new DeleteSaleCommand(id));
            return Ok(ApiResponse<string>.Ok("Sale deleted successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<string>.Fail("Internal server error"));
        }
    }
}

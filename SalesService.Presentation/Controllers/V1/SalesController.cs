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
        var result = await _mediator.Send(new GetSalesByUserQuery());
        return Ok(ApiResponse<IEnumerable<SaleDto>>.Ok(result, "Sales retrieved successfully"));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<SaleDto>>> GetSale(int id)
    {
        var result = await _mediator.Send(new GetSaleByIdQuery(id));
        if (result == null)
            return NotFound(ApiResponse<SaleDto>.Fail("Sale not found"));
        return Ok(ApiResponse<SaleDto>.Ok(result, "Sale retrieved successfully"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SaleDto>>> CreateSale([FromBody] CreateSaleDto dto)
    {
        var result = await _mediator.Send(new CreateSaleCommand(dto));
        return CreatedAtAction(nameof(GetSale), new { id = result.Id }, ApiResponse<SaleDto>.Ok(result, "Sale created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<SaleDto>>> UpdateSale(int id, [FromBody] UpdateSaleDto dto)
    {
        var result = await _mediator.Send(new UpdateSaleCommand(id, dto));
        return Ok(ApiResponse<SaleDto>.Ok(result, "Sale updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteSale(int id)
    {
        await _mediator.Send(new DeleteSaleCommand(id));
        return Ok(ApiResponse<string>.Ok("Sale deleted successfully"));
    }
}

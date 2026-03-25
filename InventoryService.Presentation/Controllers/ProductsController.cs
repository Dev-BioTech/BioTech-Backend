using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;

namespace InventoryService.Presentation.Controllers.V1;

[ApiController]

[Route("api/v1/[controller]")]

public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct(CreateProductDto dto)
    {
        try
        {
            var result = await _mediator.Send(new CreateProductCommand(dto));
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, ApiResponse<ProductDto>.Ok(result, "Product created successfully"));
        }
        catch (ArgumentException ex)
        {
            return Conflict(ApiResponse<ProductDto>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<ProductDto>.Fail("An error occurred while processing your request"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product == null)
            return NotFound(ApiResponse<ProductDto>.Fail("Product not found"));
        return Ok(ApiResponse<ProductDto>.Ok(product));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProducts([FromQuery] int farmId)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(farmId));
        return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(products));
    }

    [HttpGet("farms/{farmId}/low-stock")]
    public async Task<ActionResult<ApiResponse<IEnumerable<LowStockProductDto>>>> GetLowStockProducts(int farmId)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(farmId));
        return Ok(ApiResponse<IEnumerable<LowStockProductDto>>.Ok(products));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateProductCommand(id, dto));
            return Ok(ApiResponse<ProductDto>.Ok(result, "Product updated successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<ProductDto>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<ProductDto>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
    {
        try
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok(ApiResponse<bool>.Ok(true, "Product deleted successfully"));
        }
        catch (ArgumentException ex)
        {
            return NotFound(ApiResponse<bool>.Fail(ex.Message));
        }
    }
}

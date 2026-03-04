using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var result = await _mediator.Send(new CreateProductCommand(dto));
        return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int farmId)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(farmId));
        return Ok(products);
    }

    [HttpGet("farms/{farmId}/low-stock")]
    public async Task<ActionResult<IEnumerable<LowStockProductDto>>> GetLowStockProducts(int farmId)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(farmId));
        return Ok(products);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateProductCommand(id, dto));
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
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        try
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

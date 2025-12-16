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
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateProduct(CreateProductDto dto)
    {
        var id = await _mediator.Send(new CreateProductCommand(dto));
        return CreatedAtAction(nameof(GetProducts), new { id }, id); // Ideally GetById, but GetProducts is fine for now
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int farmId)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(farmId));
        return Ok(products);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStockProducts([FromQuery] int farmId)
    {
        var products = await _mediator.Send(new GetLowStockProductsQuery(farmId));
        return Ok(products);
    }
}

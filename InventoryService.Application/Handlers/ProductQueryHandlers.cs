using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using InventoryService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Handlers;

public class ProductQueryHandlers :
    IRequestHandler<GetLowStockProductsQuery, IEnumerable<ProductDto>>,
    IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repository;

    public ProductQueryHandlers(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetLowStockAsync(request.FarmId, cancellationToken);
        return MapToDto(products);
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync(request.FarmId, cancellationToken);
        return MapToDto(products);
    }

    private static IEnumerable<ProductDto> MapToDto(IEnumerable<Domain.Entities.Product> products)
    {
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            FarmId = p.FarmId,
            Name = p.Name,
            Category = p.Category?.ToString(),
            UnitOfMeasure = p.UnitOfMeasure,
            CurrentQuantity = p.CurrentQuantity,
            AverageCost = p.AverageCost,
            MinimumStock = p.MinimumStock,
            Active = p.Active
        });
    }
}

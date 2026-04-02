using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Enums;
using InventoryService.Domain.Interfaces;
using MediatR;

namespace InventoryService.Application.Queries;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(request.FarmId, cancellationToken);
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Category = (int?)p.Category,
            UnitOfMeasure = p.UnitOfMeasure,
            CurrentQuantity = p.CurrentQuantity,
            AverageCost = p.AverageCost,
            MinimumStock = p.MinimumStock,
            FarmId = p.FarmId,
            Active = p.Active
        });
    }
}

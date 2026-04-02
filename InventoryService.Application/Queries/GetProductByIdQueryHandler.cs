using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using MediatR;

namespace InventoryService.Application.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Category = (int?)product.Category,
            UnitOfMeasure = product.UnitOfMeasure,
            CurrentQuantity = product.CurrentQuantity,
            AverageCost = product.AverageCost,
            MinimumStock = product.MinimumStock,
            FarmId = product.FarmId,
            Active = product.Active
        };
    }
}

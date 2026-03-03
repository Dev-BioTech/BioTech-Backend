using MediatR;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;

namespace InventoryService.Application.Queries;

public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, IEnumerable<LowStockProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetLowStockProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<LowStockProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetLowStockAsync(request.FarmId, cancellationToken);
        
        return products
            .Where(p => p.CurrentQuantity <= p.MinimumStock)
            .Take(5) // Limit to 5 records maximum
            .Select(product => new LowStockProductDto(
                product.Id,
                product.Name,
                product.CurrentQuantity,
                product.MinimumStock
            ));
    }
}

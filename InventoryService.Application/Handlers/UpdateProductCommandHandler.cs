using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ArgumentException($"Product with ID {request.Id} not found");
        }

        // Check if name conflicts with another product
        var existingProduct = await _repository.GetAllAsync(product.FarmId, cancellationToken);
        if (existingProduct.Any(p => p.Name == request.Dto.Name && p.Id != request.Id))
        {
            throw new ArgumentException($"Product with Name '{request.Dto.Name}' already exists in this farm");
        }

        // Update properties
        product.Name = request.Dto.Name;
        product.Category = request.Dto.Category;
        
        if (!string.IsNullOrWhiteSpace(request.Dto.UnitOfMeasure))
        {
            product.UnitOfMeasure = request.Dto.UnitOfMeasure;
        }
        
        if (request.Dto.MinimumStock.HasValue)
        {
            product.MinimumStock = request.Dto.MinimumStock.Value;
        }

        if (request.Dto.CurrentQuantity.HasValue)
        {
            product.CurrentQuantity = request.Dto.CurrentQuantity.Value;
        }

        if (request.Dto.AverageCost.HasValue)
        {
            product.AverageCost = request.Dto.AverageCost.Value;
        }

        await _repository.UpdateAsync(product, cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            FarmId = product.FarmId,
            Name = product.Name,
            Category = product.Category?.ToString(),
            UnitOfMeasure = product.UnitOfMeasure,
            CurrentQuantity = product.CurrentQuantity,
            AverageCost = product.AverageCost,
            MinimumStock = product.MinimumStock,
            Active = product.Active
        };
    }
}

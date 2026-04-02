using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Enums;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (await _repository.ExistsAsync(dto.Name, dto.FarmId, cancellationToken))
        {
            throw new ArgumentException($"Product with Name '{dto.Name}' already exists in Farm {dto.FarmId}");
        }

        var product = new Product
        {
            FarmId = dto.FarmId,
            Name = dto.Name,
            Category = dto.Category.HasValue ? (ProductCategory)dto.Category.Value : null,
            UnitOfMeasure = dto.UnitOfMeasure,
            CurrentQuantity = dto.CurrentQuantity,
            AverageCost = dto.AverageCost,
            MinimumStock = dto.MinimumStock,
            Active = true
        };

        var createdId = await _repository.AddAsync(product, cancellationToken);
        
        return new ProductDto
        {
            Id = createdId,
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

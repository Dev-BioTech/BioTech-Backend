using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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
            Category = dto.Category,
            UnitOfMeasure = dto.UnitOfMeasure,
            CurrentQuantity = dto.CurrentQuantity,
            AverageCost = dto.AverageCost,
            MinimumStock = dto.MinimumStock,
            Active = true
        };

        return await _repository.AddAsync(product, cancellationToken);
    }
}

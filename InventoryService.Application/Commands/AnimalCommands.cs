using InventoryService.Application.DTOs;
using InventoryService.Domain.Entities;
using InventoryService.Domain.Enums;
using InventoryService.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Commands;

public record RegisterAnimalCommand(CreateAnimalDto Dto) : IRequest<long>;

public class RegisterAnimalCommandHandler : IRequestHandler<RegisterAnimalCommand, long>
{
    private readonly IAnimalRepository _repository;

    public RegisterAnimalCommandHandler(IAnimalRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(RegisterAnimalCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (await _repository.ExistsAsync(dto.VisualCode, dto.FarmId, cancellationToken))
        {
            throw new ArgumentException($"Animal with Visual Code {dto.VisualCode} already exists in Farm {dto.FarmId}");
        }

        var animal = new Animal
        {
            FarmId = dto.FarmId,
            VisualCode = dto.VisualCode,
            ElectronicCode = dto.ElectronicCode,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            Sex = dto.Sex,
            CurrentStatus = "ACTIVE",
            BreedId = dto.BreedId,
            BirthDate = dto.BirthDate,
            MotherId = dto.MotherId,
            FatherId = dto.FatherId,
            InitialCost = dto.InitialCost,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.AddAsync(animal, cancellationToken);
    }
}

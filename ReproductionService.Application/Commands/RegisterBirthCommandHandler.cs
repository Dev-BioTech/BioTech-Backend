using MediatR;
using ReproductionService.Application.Interfaces;
using ReproductionService.Application.DTOs;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Commands;

public class RegisterBirthCommandHandler : IRequestHandler<RegisterBirthCommand, BirthDto>
{
    private readonly IBirthRepository _birthRepository;

    public RegisterBirthCommandHandler(IBirthRepository birthRepository)
    {
        _birthRepository = birthRepository;
    }

    public async Task<BirthDto> Handle(RegisterBirthCommand request, CancellationToken cancellationToken)
    {
        var birth = new Birth(
            request.MotherAnimalId,
            0, // FarmId will be set in the controller after validation
            request.OffspringTag,
            request.BirthDate,
            request.Weight,
            request.Gender
        );
        
        var createdBirth = await _birthRepository.AddAsync(birth, cancellationToken);
        
        return new BirthDto(
            createdBirth.MotherAnimalId,
            createdBirth.OffspringTag,
            createdBirth.BirthDate,
            createdBirth.Weight,
            createdBirth.Gender
        );
    }
}

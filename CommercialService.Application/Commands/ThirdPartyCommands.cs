using MediatR;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace CommercialService.Application.Commands;

// Create
public record CreateThirdPartyCommand(CreateThirdPartyDto Dto) : IRequest<long>;

public class CreateThirdPartyCommandHandler : IRequestHandler<CreateThirdPartyCommand, long>
{
    private readonly IThirdPartyRepository _repository;

    public CreateThirdPartyCommandHandler(IThirdPartyRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> Handle(CreateThirdPartyCommand request, CancellationToken cancellationToken)
    {
        // Validation: Check if document exists for farm
        if (await _repository.ExistsAsync(request.Dto.FarmId, request.Dto.IdentityDocument, cancellationToken))
        {
            throw new ArgumentException($"A third party with document {request.Dto.IdentityDocument} already exists for this farm.");
        }

        var thirdParty = new ThirdParty
        {
            FarmId = request.Dto.FarmId,
            FullName = request.Dto.FullName,
            IdentityDocument = request.Dto.IdentityDocument,
            Phone = request.Dto.Phone,
            Email = request.Dto.Email,
            IsSupplier = request.Dto.IsSupplier,
            IsCustomer = request.Dto.IsCustomer,
            IsEmployee = request.Dto.IsEmployee,
            IsVeterinarian = request.Dto.IsVeterinarian,
            Address = request.Dto.Address
        };

        await _repository.AddAsync(thirdParty, cancellationToken);
        return thirdParty.Id;
    }
}

// Update
public record UpdateThirdPartyCommand(long Id, UpdateThirdPartyDto Dto) : IRequest<bool>;

public class UpdateThirdPartyCommandHandler : IRequestHandler<UpdateThirdPartyCommand, bool>
{
    private readonly IThirdPartyRepository _repository;

    public UpdateThirdPartyCommandHandler(IThirdPartyRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateThirdPartyCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null) return false;

        // Update fields
        existing.FullName = request.Dto.FullName;
        // existing.Data typo removed
        existing.Phone = request.Dto.Phone;
        existing.Email = request.Dto.Email;
        existing.IsSupplier = request.Dto.IsSupplier;
        existing.IsCustomer = request.Dto.IsCustomer;
        existing.IsEmployee = request.Dto.IsEmployee;
        existing.IsVeterinarian = request.Dto.IsVeterinarian;
        existing.Address = request.Dto.Address;

        // Note: IdentifyDocument usually shouldn't change, but if needed we'd need to check conflict again.
        // For now let's assume IdentityDocument is immutable or carefully changed.

        await _repository.UpdateAsync(existing, cancellationToken);
        return true;
    }
}

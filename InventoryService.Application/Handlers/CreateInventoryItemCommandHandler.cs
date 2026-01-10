using System.Threading;
using System.Threading.Tasks;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using InventoryService.Domain.Entities;
using MediatR;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Handlers;

public class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, InventoryItemDto>
{
    private readonly IInventoryRepository _repository;
    private readonly Shared.Infrastructure.Interfaces.IMessenger _messenger;

    public CreateInventoryItemCommandHandler(IInventoryRepository repository, Shared.Infrastructure.Interfaces.IMessenger messenger)
    {
        _repository = repository;
        _messenger = messenger;
    }

    public async Task<InventoryItemDto> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        // Validate FarmId via HerdService
        var paddocks = await _messenger.GetAsync<IEnumerable<object>>("HerdService", $"/api/v1/paddocks/farm/{request.FarmId}", cancellationToken);
        
        // If the call fails (throws) or returns null/empty, we assume invalid farm or connection issue.
        // For strict validation, we might want to check if the list is not null. 
        // However, a farm might have 0 paddocks but still exist. 
        // But the requirement implies we want to check validity. 
        // If the service returns 404, HttpMessenger throws. 
        // If it returns 200 OK with empty list, it means farm exists (or at least the query ran).
        // Let's assume if we get a response, it's valid.

        var item = new InventoryItem(request.Name, request.Quantity, request.Unit, request.FarmId);
        await _repository.AddAsync(item, cancellationToken);

        return new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            Unit = item.Unit,
            FarmId = item.FarmId,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}

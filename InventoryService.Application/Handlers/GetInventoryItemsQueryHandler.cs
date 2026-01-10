using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using MediatR;
using InventoryService.Application.Queries;

namespace InventoryService.Application.Handlers;

public class GetInventoryItemsQueryHandler : IRequestHandler<GetInventoryItemsQuery, IEnumerable<InventoryItemDto>>
{
    private readonly IInventoryRepository _repository;

    public GetInventoryItemsQueryHandler(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InventoryItemDto>> Handle(GetInventoryItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetByFarmIdAsync(request.FarmId, request.Page, request.PageSize, cancellationToken);

        return items.Select(item => new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            Unit = item.Unit,
            FarmId = item.FarmId,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        });
    }
}

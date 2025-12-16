using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Queries;

public record GetMovementsByProductQuery(int ProductId) : IRequest<IEnumerable<InventoryMovementDto>>;

public class GetMovementsByProductQueryHandler : IRequestHandler<GetMovementsByProductQuery, IEnumerable<InventoryMovementDto>>
{
    private readonly IInventoryRepository _repository;

    public GetMovementsByProductQueryHandler(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InventoryMovementDto>> Handle(GetMovementsByProductQuery request, CancellationToken cancellationToken)
    {
        var movements = await _repository.GetMovementsByProductIdAsync(request.ProductId, cancellationToken);

        return movements.Select(m => new InventoryMovementDto
        {
            Id = m.Id,
            ProductId = m.ProductId,
            ProductName = m.Product?.Name ?? "Unknown",
            Direction = m.MovementType.ToString(),
            Concept = m.Concept.ToString(),
            Quantity = m.Quantity,
            MovementDate = m.MovementDate,
            ReferenceDocument = m.ReferenceDocument,
            Observations = m.Observations,
            TransactionUnitCost = m.TransactionUnitCost,
            TransactionTotalCost = m.TransactionTotalCost,
            SubsequentQuantityBalance = m.SubsequentQuantityBalance,
            SubsequentAverageCost = m.SubsequentAverageCost,
            RegisteredBy = m.RegisteredBy
        });
    }
}

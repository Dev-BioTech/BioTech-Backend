using MediatR;
using CommercialService.Application.Commands;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommercialService.Application.Handlers;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, long>
{
    private readonly ICommercialRepository _repository;
    private readonly IPublisher _publisher;

    public CreateTransactionCommandHandler(ICommercialRepository repository, IPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<long> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var transaction = new CommercialTransaction
        {
            FarmId = dto.FarmId,
            ThirdPartyId = dto.ThirdPartyId,
            TransactionType = dto.TransactionType,
            TransactionDate = dto.TransactionDate,
            InvoiceNumber = dto.InvoiceNumber,
            Subtotal = dto.Subtotal,
            Taxes = dto.Taxes,
            Discounts = dto.Discounts,
            NetTotal = (dto.Subtotal + dto.Taxes) - dto.Discounts,
            PaymentStatus = dto.PaymentStatus,
            Observations = dto.Observations,
            RegisteredBy = request.UserId,
            RegisteredAt = System.DateTime.UtcNow
        };

        if (dto.AnimalDetails != null)
        {
            foreach (var animal in dto.AnimalDetails)
            {
                var lineTotal = (animal.BaseHeadPrice + animal.CommissionCost + animal.TransportCost);
                transaction.AnimalDetails.Add(new TransactionAnimalDetail
                {
                    AnimalId = animal.AnimalId,
                    PricePerKilo = animal.PricePerKilo,
                    WeightAtNegotiation = animal.WeightAtNegotiation,
                    BaseHeadPrice = animal.BaseHeadPrice,
                    CommissionCost = animal.CommissionCost,
                    TransportCost = animal.TransportCost,
                    FinalLineValue = lineTotal
                    // AnimalMovementId would be linked in a richer implementation with domain services
                });
            }
        }

        if (dto.ProductDetails != null)
        {
            foreach (var product in dto.ProductDetails)
            {
                var lineTotal = product.Quantity * product.UnitPrice;
                transaction.ProductDetails.Add(new TransactionProductDetail
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    UnitPrice = product.UnitPrice,
                    LineSubtotal = lineTotal
                    // InventoryMovementId would be linked in a richer implementation
                });
            }
        }

        // Recalculate NetTotal if needed or rely on DTO. 
        // For now, trusting DTO but ensuring subtotal logic is adhered to in real business logic.
        // Simple logic:
        if (transaction.NetTotal == null || transaction.NetTotal == 0)
        {
            decimal animalSum = transaction.AnimalDetails.Sum(a => a.FinalLineValue ?? 0);
            decimal productSum = transaction.ProductDetails.Sum(p => p.LineSubtotal ?? 0);
            // Override Dtos if empty? 
            // Keeping DTO values for now as per minimal implementation.
        }

        await _repository.AddTransactionAsync(transaction, cancellationToken);

        // Publish Domain Event
        var items = new List<Domain.Events.TransactionItemDto>();
        items.AddRange(transaction.AnimalDetails.Select(a => new Domain.Events.TransactionItemDto("Animal", a.AnimalId, 1, a.FinalLineValue ?? a.BaseHeadPrice, a.FinalLineValue ?? a.BaseHeadPrice)));
        items.AddRange(transaction.ProductDetails.Select(p => new Domain.Events.TransactionItemDto("Product", p.ProductId, p.Quantity, p.UnitPrice, p.LineSubtotal ?? 0)));

        await _publisher.Publish(new Domain.Events.TransactionCreatedEvent(
            transaction.Id,
            transaction.FarmId,
            transaction.TransactionType,
            transaction.TransactionDate,
            items
        ), cancellationToken);

        return transaction.Id;
    }
}

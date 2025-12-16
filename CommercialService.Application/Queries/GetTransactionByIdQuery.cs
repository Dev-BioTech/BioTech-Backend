using MediatR;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CommercialService.Application.Queries;

public record GetTransactionByIdQuery(long Id) : IRequest<TransactionFullDto?>;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionFullDto?>
{
    private readonly ICommercialRepository _repository;

    public GetTransactionByIdQueryHandler(ICommercialRepository repository)
    {
        _repository = repository;
    }

    public async Task<TransactionFullDto?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var t = await _repository.GetTransactionByIdAsync(request.Id, cancellationToken);
        if (t == null) return null;

        return new TransactionFullDto
        {
            Id = t.Id,
            FarmId = t.FarmId,
            ThirdPartyId = t.ThirdPartyId,
            TransactionType = t.TransactionType,
            TransactionDate = t.TransactionDate,
            InvoiceNumber = t.InvoiceNumber,
            NetTotal = t.NetTotal ?? 0,
            PaymentStatus = t.PaymentStatus,
            Subtotal = t.Subtotal,
            Taxes = t.Taxes,
            Discounts = t.Discounts,
            Observations = t.Observations,
            RegisteredAt = t.RegisteredAt,
            RegisteredBy = t.RegisteredBy
        };
    }
}

using MediatR;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Interfaces;
using CommercialService.Domain.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CommercialService.Application.Queries;

public record GetTransactionsQuery(int FarmId, System.DateTime? FromDate, System.DateTime? ToDate, TransactionType? Type) : IRequest<List<TransactionSummaryDto>>;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, List<TransactionSummaryDto>>
{
    private readonly ICommercialRepository _repository;

    public GetTransactionsQueryHandler(ICommercialRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TransactionSummaryDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetTransactionsAsync(request.FarmId, request.FromDate, request.ToDate, request.Type, cancellationToken);

        return transactions.Select(t => new TransactionSummaryDto
        {
            Id = t.Id,
            FarmId = t.FarmId,
            ThirdPartyId = t.ThirdPartyId,
            TransactionType = t.TransactionType,
            TransactionDate = t.TransactionDate,
            InvoiceNumber = t.InvoiceNumber,
            NetTotal = t.NetTotal ?? 0,
            PaymentStatus = t.PaymentStatus
        }).ToList();
    }
}

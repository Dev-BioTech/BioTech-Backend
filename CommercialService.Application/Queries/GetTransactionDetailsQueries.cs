using MediatR;
using CommercialService.Application.DTOs;
using CommercialService.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CommercialService.Application.Queries;

// Animals
public record GetTransactionAnimalsQuery(long TransactionId) : IRequest<List<TransactionAnimalDto>>;

public class GetTransactionAnimalsQueryHandler : IRequestHandler<GetTransactionAnimalsQuery, List<TransactionAnimalDto>>
{
    private readonly ICommercialRepository _repository;

    public GetTransactionAnimalsQueryHandler(ICommercialRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TransactionAnimalDto>> Handle(GetTransactionAnimalsQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetTransactionAnimalsAsync(request.TransactionId, cancellationToken);

        return details.Select(d => new TransactionAnimalDto
        {
            Id = d.Id,
            AnimalId = d.AnimalId,
            PricePerKilo = d.PricePerKilo,
            WeightAtNegotiation = d.WeightAtNegotiation,
            BaseHeadPrice = d.BaseHeadPrice,
            CommissionCost = d.CommissionCost,
            TransportCost = d.TransportCost,
            FinalLineValue = d.FinalLineValue
        }).ToList();
    }
}

// Products
public record GetTransactionProductsQuery(long TransactionId) : IRequest<List<TransactionProductDto>>;

public class GetTransactionProductsQueryHandler : IRequestHandler<GetTransactionProductsQuery, List<TransactionProductDto>>
{
    private readonly ICommercialRepository _repository;

    public GetTransactionProductsQueryHandler(ICommercialRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TransactionProductDto>> Handle(GetTransactionProductsQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetTransactionProductsAsync(request.TransactionId, cancellationToken);

        return details.Select(d => new TransactionProductDto
        {
            Id = d.Id,
            ProductId = d.ProductId,
            Quantity = d.Quantity,
            UnitPrice = d.UnitPrice,
            LineSubtotal = d.LineSubtotal
        }).ToList();
    }
}

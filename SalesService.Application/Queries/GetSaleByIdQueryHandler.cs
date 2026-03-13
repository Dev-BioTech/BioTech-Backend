using SalesService.Application.DTOs;
using SalesService.Application.Interfaces;
using MediatR;

namespace SalesService.Application.Queries;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto?>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SaleDto?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
            return null;

        return new SaleDto(
            sale.Id,
            sale.FarmId,
            sale.AnimalId,
            sale.BuyerName,
            sale.SaleDate,
            sale.Amount,
            sale.Notes,
            sale.CreatedAt
        );
    }
}

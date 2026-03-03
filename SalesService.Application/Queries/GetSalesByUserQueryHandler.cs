using MediatR;
using SalesService.Application.Interfaces;
using SalesService.Application.DTOs;
using SalesService.Domain.Entities;
using AuthService.Application.Interfaces;

namespace SalesService.Application.Queries;

public class GetSalesByUserQueryHandler : IRequestHandler<GetSalesByUserQuery, IEnumerable<SaleDto>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetSalesByUserQueryHandler(ISaleRepository saleRepository, ICurrentUserService currentUserService)
    {
        _saleRepository = saleRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<SaleDto>> Handle(GetSalesByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var sales = await _saleRepository.GetByUserIdAsync(userId.Value, cancellationToken);
        
        return sales.Select(sale => new SaleDto(
            sale.Id,
            sale.FarmId,
            sale.AnimalId,
            sale.BuyerName,
            sale.SaleDate,
            sale.Amount,
            sale.Notes,
            sale.CreatedAt
        ));
    }
}

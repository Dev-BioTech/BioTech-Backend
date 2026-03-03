using MediatR;
using SalesService.Application.Interfaces;
using SalesService.Application.DTOs;
using SalesService.Domain.Entities;
using AuthService.Application.Interfaces;

namespace SalesService.Application.Commands;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, ICurrentUserService currentUserService)
    {
        _saleRepository = saleRepository;
        _currentUserService = currentUserService;
    }

    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        var sale = new Sale(
            request.Dto.FarmId,
            request.Dto.AnimalId,
            request.Dto.BuyerName,
            request.Dto.SaleDate,
            request.Dto.Amount,
            request.Dto.Notes
        );

        var createdSale = await _saleRepository.AddAsync(sale, cancellationToken);

        return new SaleDto(
            createdSale.Id,
            createdSale.FarmId,
            createdSale.AnimalId,
            createdSale.BuyerName,
            createdSale.SaleDate,
            createdSale.Amount,
            createdSale.Notes,
            createdSale.CreatedAt
        );
    }
}

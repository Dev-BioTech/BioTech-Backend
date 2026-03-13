using MediatR;
using SalesService.Application.Interfaces;
using SalesService.Application.DTOs;
using SalesService.Domain.Entities;

namespace SalesService.Application.Commands;

public class UpdateSaleCommandHandler : IRequestHandler<UpdateSaleCommand, SaleDto>
{
    private readonly ISaleRepository _saleRepository;

    public UpdateSaleCommandHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SaleDto> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            throw new ArgumentException($"Sale with ID {request.Id} not found");
        }

        // Update properties
        sale.BuyerName = request.Dto.BuyerName;
        sale.SaleDate = request.Dto.SaleDate;
        sale.Amount = request.Dto.Amount;
        sale.UpdatedAt = DateTime.UtcNow;

        await _saleRepository.UpdateAsync(sale, cancellationToken);

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

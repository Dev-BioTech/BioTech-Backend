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

        // Update properties only if provided
        if (!string.IsNullOrWhiteSpace(request.Dto.BuyerName))
            sale.BuyerName = request.Dto.BuyerName;

        if (request.Dto.SaleDate.HasValue && request.Dto.SaleDate.Value != DateTime.MinValue)
            sale.SaleDate = request.Dto.SaleDate.Value;

        if (request.Dto.Amount.HasValue)
            sale.Amount = request.Dto.Amount.Value;

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

using MediatR;
using SalesService.Application.Interfaces;

namespace SalesService.Application.Commands;

public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand>
{
    private readonly ISaleRepository _saleRepository;

    public DeleteSaleCommandHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (sale == null)
        {
            throw new ArgumentException($"Sale with ID {request.Id} not found");
        }

        await _saleRepository.DeleteAsync(request.Id, cancellationToken);
    }
}

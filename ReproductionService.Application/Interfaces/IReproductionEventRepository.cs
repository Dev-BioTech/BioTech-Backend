using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Interfaces;

public interface IReproductionEventRepository
{
    Task<ReproductionEvent> AddAsync(ReproductionEvent reproductionEvent, CancellationToken cancellationToken);
    Task<ReproductionEvent?> GetByIdAsync(long id, CancellationToken cancellationToken);
    // Add other methods as needed
}

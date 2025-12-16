using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IBatchRepository
{
    Task<Batch> AddAsync(Batch batch, CancellationToken cancellationToken);
    Task<IEnumerable<Batch>> GetAllAsync(CancellationToken cancellationToken);
    Task<Batch?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

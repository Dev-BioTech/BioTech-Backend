using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IPaddockRepository
{
    Task<Paddock> AddAsync(Paddock paddock, CancellationToken cancellationToken);
    Task<IEnumerable<Paddock>> GetAllAsync(CancellationToken cancellationToken);
    Task<Paddock?> GetByIdAsync(int id, CancellationToken cancellationToken);
}

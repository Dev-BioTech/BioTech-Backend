using SalesService.Domain.Entities;

namespace SalesService.Application.Interfaces;

public interface ISaleRepository
{
    Task<IEnumerable<Sale>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken);
    Task UpdateAsync(Sale sale, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IFarmRepository
{
    Task<Farm?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Farm>> GetAllByTenantIdAsync(int tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(Farm farm, CancellationToken cancellationToken = default);
    Task UpdateAsync(Farm farm, CancellationToken cancellationToken = default);
    Task DeleteAsync(Farm farm, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}

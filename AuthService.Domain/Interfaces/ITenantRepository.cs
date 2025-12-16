using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default);
    // Delete might be logical delete (Active = false), handled in Update usually, but explicit Delete is good for interfaces
    Task DeleteAsync(Tenant tenant, CancellationToken cancellationToken = default); 
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}

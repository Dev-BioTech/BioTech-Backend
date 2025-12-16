using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface IFarmRepository
{
    Task<Farm?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Farm>> GetByTenantUserIdAsync(int tenantUserId, bool includeInactive, CancellationToken ct = default);
    Task<Farm> AddAsync(Farm farm, int? userId, CancellationToken ct = default);
    Task UpdateAsync(Farm farm, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommercialService.Domain.Entities;

namespace CommercialService.Domain.Interfaces;

public interface IThirdPartyRepository
{
    Task AddAsync(ThirdParty thirdParty, CancellationToken cancellationToken);
    Task UpdateAsync(ThirdParty thirdParty, CancellationToken cancellationToken);
    Task<ThirdParty?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<ThirdParty>> GetAllAsync(int farmId, bool? isSupplier, bool? isCustomer, int page, int pageSize, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int farmId, string identityDocument, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}


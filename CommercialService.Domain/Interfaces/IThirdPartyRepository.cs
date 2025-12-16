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
    Task<List<ThirdParty>> GetAllAsync(int farmId, bool? isSupplier, bool? isCustomer, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(int farmId, string identityDocument, CancellationToken cancellationToken);
}

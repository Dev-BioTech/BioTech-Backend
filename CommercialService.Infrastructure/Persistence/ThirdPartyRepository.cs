using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialService.Infrastructure.Persistence;

public class ThirdPartyRepository : IThirdPartyRepository
{
    private readonly CommercialDbContext _context;

    public ThirdPartyRepository(CommercialDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ThirdParty thirdParty, CancellationToken cancellationToken)
    {
        await _context.ThirdParties.AddAsync(thirdParty, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ThirdParty thirdParty, CancellationToken cancellationToken)
    {
        _context.ThirdParties.Update(thirdParty);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ThirdParty?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.ThirdParties.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<ThirdParty>> GetAllAsync(int farmId, bool? isSupplier, bool? isCustomer, CancellationToken cancellationToken)
    {
        var query = _context.ThirdParties.AsNoTracking().Where(tp => tp.FarmId == farmId);

        if (isSupplier.HasValue && isSupplier.Value)
        {
            query = query.Where(tp => tp.IsSupplier);
        }

        if (isCustomer.HasValue && isCustomer.Value)
        {
            query = query.Where(tp => tp.IsCustomer);
        }

        return await query.OrderBy(tp => tp.FullName).ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int farmId, string identityDocument, CancellationToken cancellationToken)
    {
        return await _context.ThirdParties
            .AnyAsync(tp => tp.FarmId == farmId && tp.IdentityDocument == identityDocument, cancellationToken);
    }
}

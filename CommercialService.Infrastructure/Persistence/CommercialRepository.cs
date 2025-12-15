using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Enums;
using CommercialService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialService.Infrastructure.Persistence;

public class CommercialRepository : ICommercialRepository
{
    private readonly CommercialDbContext _context;

    public CommercialRepository(CommercialDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionAsync(CommercialTransaction transaction, CancellationToken cancellationToken)
    {
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<CommercialTransaction?> GetTransactionByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<CommercialTransaction>> GetTransactionsAsync(int farmId, DateTime? fromDate, DateTime? toDate, TransactionType? type, CancellationToken cancellationToken)
    {
        var query = _context.Transactions.AsNoTracking().Where(t => t.FarmId == farmId);

        if (fromDate.HasValue)
            query = query.Where(t => t.TransactionDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(t => t.TransactionDate <= toDate.Value);

        if (type.HasValue)
            query = query.Where(t => t.TransactionType == type.Value);

        return await query.OrderByDescending(t => t.TransactionDate).ToListAsync(cancellationToken);
    }

    public async Task<List<TransactionAnimalDetail>> GetTransactionAnimalsAsync(long transactionId, CancellationToken cancellationToken)
    {
        return await _context.AnimalDetails
            .AsNoTracking()
            .Where(ad => ad.TransactionId == transactionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TransactionProductDetail>> GetTransactionProductsAsync(long transactionId, CancellationToken cancellationToken)
    {
        return await _context.ProductDetails
            .AsNoTracking()
            .Where(pd => pd.TransactionId == transactionId)
            .ToListAsync(cancellationToken);
    }
}

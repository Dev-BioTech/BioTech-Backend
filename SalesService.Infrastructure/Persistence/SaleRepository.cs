using Microsoft.EntityFrameworkCore;
using SalesService.Application.Interfaces;
using SalesService.Domain.Entities;

namespace SalesService.Infrastructure.Persistence;

public class SaleRepository : ISaleRepository
{
    private readonly SalesDbContext _context;

    public SaleRepository(SalesDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sale>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .Where(s => s.CreatedBy == userId && s.IsActive)
            .OrderByDescending(s => s.SaleDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken)
    {
        sale.UpdatedAt = DateTime.UtcNow;
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale != null)
        {
            sale.IsActive = false;
            sale.UpdatedAt = DateTime.UtcNow;
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

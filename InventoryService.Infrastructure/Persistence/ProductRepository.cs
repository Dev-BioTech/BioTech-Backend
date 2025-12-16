using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int farmId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.FarmId == farmId && p.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync(int farmId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Where(p => p.FarmId == farmId && p.Active && p.CurrentQuantity < p.MinimumStock)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string name, int farmId, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(p => p.Name == name && p.FarmId == farmId, cancellationToken);
    }
}

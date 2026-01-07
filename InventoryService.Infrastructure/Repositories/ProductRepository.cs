using InventoryService.Domain.Entities;
using InventoryService.Domain.Interfaces;
using InventoryService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(int farmId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.FarmId == farmId && p.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync(int farmId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.FarmId == farmId && p.Active && p.CurrentQuantity < p.MinimumStock)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product.Id;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string name, int farmId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .AnyAsync(p => p.Name == name && p.FarmId == farmId, cancellationToken);
    }
}

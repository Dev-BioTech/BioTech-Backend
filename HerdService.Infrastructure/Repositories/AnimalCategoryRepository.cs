using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class AnimalCategoryRepository : IAnimalCategoryRepository
{
    private readonly HerdDbContext _context;

    public AnimalCategoryRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<AnimalCategory> AddAsync(AnimalCategory category, CancellationToken cancellationToken)
    {
        await _context.AnimalCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<IEnumerable<AnimalCategory>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.AnimalCategories.ToListAsync(cancellationToken);
    }

    public async Task<AnimalCategory?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.AnimalCategories.FindAsync(new object[] { id }, cancellationToken);
    }
}

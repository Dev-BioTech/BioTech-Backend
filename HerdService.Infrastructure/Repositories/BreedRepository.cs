using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using HerdService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HerdService.Infrastructure.Repositories;

public class BreedRepository : IBreedRepository
{
    private readonly HerdDbContext _context;

    public BreedRepository(HerdDbContext context)
    {
        _context = context;
    }

    public async Task<Breed> AddAsync(Breed breed, CancellationToken cancellationToken)
    {
        await _context.Breeds.AddAsync(breed, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return breed;
    }

    public async Task<IEnumerable<Breed>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Breeds.ToListAsync(cancellationToken);
    }

    public async Task<Breed?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Breeds.FindAsync(new object[] { id }, cancellationToken);
    }
}

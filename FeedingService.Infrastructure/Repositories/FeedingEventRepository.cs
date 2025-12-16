using FeedingService.Application.Interfaces;
using FeedingService.Domain.Entities;
using FeedingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FeedingService.Infrastructure.Repositories;

public class FeedingEventRepository : IFeedingEventRepository
{
    private readonly FeedingDbContext _context;

    public FeedingEventRepository(FeedingDbContext context)
    {
        _context = context;
    }

    public async Task<FeedingEvent?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.FeedingEvents
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, ct);
    }

    public async Task<IEnumerable<FeedingEvent>> GetByFarmIdAsync(
        int farmId, 
        DateTime? from, 
        DateTime? to, 
        CancellationToken ct = default)
    {
        var query = _context.FeedingEvents
            .AsNoTracking()
            .Where(f => f.FarmId == farmId);

        if (from.HasValue)
            query = query.Where(f => f.SupplyDate >= from.Value.Date);

        if (to.HasValue)
            query = query.Where(f => f.SupplyDate <= to.Value.Date);

        return await query
            .OrderByDescending(f => f.SupplyDate)
            .ThenByDescending(f => f.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FeedingEvent>> GetByBatchIdAsync(
        int batchId, 
        CancellationToken ct = default)
    {
        return await _context.FeedingEvents
            .AsNoTracking()
            .Where(f => f.BatchId == batchId)
            .OrderByDescending(f => f.SupplyDate)
            .ThenByDescending(f => f.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FeedingEvent>> GetByAnimalIdAsync(
        long animalId, 
        CancellationToken ct = default)
    {
        return await _context.FeedingEvents
            .AsNoTracking()
            .Where(f => f.AnimalId == animalId)
            .OrderByDescending(f => f.SupplyDate)
            .ThenByDescending(f => f.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<FeedingEvent>> GetByProductIdAsync(
        int productId, 
        CancellationToken ct = default)
    {
        return await _context.FeedingEvents
            .AsNoTracking()
            .Where(f => f.ProductId == productId)
            .OrderByDescending(f => f.SupplyDate)
            .ThenByDescending(f => f.Id)
            .ToListAsync(ct);
    }

    public async Task<FeedingEvent> AddAsync(
        FeedingEvent feedingEvent, 
        CancellationToken ct = default)
    {
        await _context.FeedingEvents.AddAsync(feedingEvent, ct);
        await _context.SaveChangesAsync(ct);
        return feedingEvent;
    }

    public async Task UpdateAsync(
        FeedingEvent feedingEvent, 
        CancellationToken ct = default)
    {
        _context.FeedingEvents.Update(feedingEvent);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        var entity = await _context.FeedingEvents.FindAsync(new object[] { id }, ct);
        
        if (entity != null)
        {
            _context.FeedingEvents.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
using HealthService.Application.Interfaces;
using HealthService.Domain.Entities;
using HealthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthService.Infrastructure.Repositories;

public class HealthEventRepository : IHealthEventRepository
{
    private readonly HealthServiceDbContext _context;

    public HealthEventRepository(HealthServiceDbContext context)
    {
        _context = context;
    }

    public async Task<HealthEvent?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await _context.HealthEvents
            .FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetByFarmIdAsync(
        int farmId, 
        DateOnly? fromDate, 
        DateOnly? toDate,
        string? eventType,
        CancellationToken ct = default)
    {
        var query = _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.FarmId == farmId);

        if (fromDate.HasValue)
            query = query.Where(h => h.EventDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(h => h.EventDate <= toDate.Value);

        if (!string.IsNullOrWhiteSpace(eventType))
            query = query.Where(h => h.EventType == eventType);

        return await query
            .OrderByDescending(h => h.EventDate)
            .ThenByDescending(h => h.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetByAnimalIdAsync(long animalId, string? eventType, CancellationToken ct = default)
    {
        var query = _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.AnimalId == animalId);

        if (!string.IsNullOrWhiteSpace(eventType))
            query = query.Where(h => h.EventType == eventType);

        return await query
            .OrderByDescending(h => h.EventDate)
            .ThenByDescending(h => h.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetByBatchIdAsync(int batchId, string? eventType, CancellationToken ct = default)
    {
        var query = _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.BatchId == batchId);

        if (!string.IsNullOrWhiteSpace(eventType))
            query = query.Where(h => h.EventType == eventType);

        return await query
            .OrderByDescending(h => h.EventDate)
            .ThenByDescending(h => h.Id)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetByEventTypeAsync(
        string eventType, 
        int farmId,
        DateOnly? fromDate, 
        DateOnly? toDate,
        CancellationToken ct = default)
    {
        var query = _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.EventType == eventType && h.FarmId == farmId);

        if (fromDate.HasValue)
            query = query.Where(h => h.EventDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(h => h.EventDate <= toDate.Value);

        return await query
            .OrderByDescending(h => h.EventDate)
            .ThenByDescending(h => h.Id)
            .ToListAsync(ct);
    }

    public async Task<HealthEvent> AddAsync(HealthEvent healthEvent, CancellationToken ct = default)
    {
        await _context.HealthEvents.AddAsync(healthEvent, ct);
        await _context.SaveChangesAsync(ct);
        return healthEvent;
    }

    public async Task UpdateAsync(HealthEvent healthEvent, CancellationToken ct = default)
    {
        _context.HealthEvents.Update(healthEvent);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(HealthEvent healthEvent, CancellationToken ct = default)
    {
        _context.HealthEvents.Remove(healthEvent);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> GetTotalEventsCountAsync(int farmId, CancellationToken ct = default)
    {
        return await _context.HealthEvents
            .CountAsync(h => h.FarmId == farmId, ct);
    }

    public async Task<decimal> GetTotalCostAsync(int farmId, CancellationToken ct = default)
    {
        return await _context.HealthEvents
            .Where(h => h.FarmId == farmId && h.Cost.HasValue)
            .SumAsync(h => h.Cost!.Value, ct);
    }

    public async Task<int> GetSickAnimalsCountAsync(int farmId, CancellationToken ct = default)
    {
        // Logic: Count unique animals that have had a "Disease" or "Treatment" event in the last 30 days
        var thirtyDaysAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        return await _context.HealthEvents
            .Where(h => h.FarmId == farmId && 
                        (h.EventType == "Disease" || h.EventType == "Treatment") &&
                        h.EventDate >= thirtyDaysAgo &&
                        h.AnimalId.HasValue)
            .Select(h => h.AnimalId)
            .Distinct()
            .CountAsync(ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetUpcomingEventsAsync(int farmId, int limit, CancellationToken ct = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return await _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.FarmId == farmId && 
                        h.RequiresFollowUp && 
                        h.NextFollowUpDate.HasValue && 
                        h.NextFollowUpDate >= today)
            .OrderBy(h => h.NextFollowUpDate)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<HealthEvent>> GetRecentTreatmentsAsync(int farmId, int limit, CancellationToken ct = default)
    {
        return await _context.HealthEvents
            .AsNoTracking()
            .Where(h => h.FarmId == farmId && 
                        (h.EventType == "Treatment" || h.EventType == "Medication" || h.EventType == "Vaccination"))
            .OrderByDescending(h => h.EventDate)
            .ThenByDescending(h => h.Id)
            .Take(limit)
            .ToListAsync(ct);
    }
}

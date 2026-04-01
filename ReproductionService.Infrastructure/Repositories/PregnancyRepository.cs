using Microsoft.EntityFrameworkCore;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;
using ReproductionService.Infrastructure.Persistence;

namespace ReproductionService.Infrastructure.Repositories;

public class PregnancyRepository : IPregnancyRepository
{
    private readonly ReproductionDbContext _context;

    public PregnancyRepository(ReproductionDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pregnancy>> GetByFarmIdAsync(int farmId, CancellationToken cancellationToken)
    {
        // Derive Pregnancies dynamically from ReproductionEvents that are successful
        var events = await _context.ReproductionEvents
            .Where(e => e.FarmId == farmId && 
                        e.ExpectedBirthDate.HasValue && 
                        (e.IsSuccessful == true || e.EventType == ReproductionService.Domain.Enums.ReproductionEventType.PregnancyCheck) && 
                        !e.IsCancelled)
            .ToListAsync(cancellationToken);
            
        return events.Select(e => new Pregnancy(e.AnimalId, e.FarmId, e.ExpectedBirthDate.Value)).ToList();
    }

    public Task<Pregnancy> AddAsync(Pregnancy pregnancy, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Derived dynamically from ReproductionEvents currently. Use CreateReproductionEventCommand.");
    }
}

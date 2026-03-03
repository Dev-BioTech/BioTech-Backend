using MediatR;
using ReproductionService.Application.Interfaces;
using ReproductionService.Application.DTOs;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries;

public class GetPregnanciesByFarmQueryHandler : IRequestHandler<GetPregnanciesByFarmQuery, IEnumerable<PregnancyDto>>
{
    private readonly IPregnancyRepository _pregnancyRepository;

    public GetPregnanciesByFarmQueryHandler(IPregnancyRepository pregnancyRepository)
    {
        _pregnancyRepository = pregnancyRepository;
    }

    public async Task<IEnumerable<PregnancyDto>> Handle(GetPregnanciesByFarmQuery request, CancellationToken cancellationToken)
    {
        var pregnancies = await _pregnancyRepository.GetByFarmIdAsync(request.FarmId, cancellationToken);
        
        var result = new List<PregnancyDto>();
        
        foreach (var pregnancy in pregnancies.Where(p => p.IsActive))
        {
            // Get animal tag (assuming we have access to animal data)
            // For now, we'll use a placeholder - in real implementation this would come from AnimalRepository
            var animalTag = $"Animal-{pregnancy.AnimalId}";
            
            // Calculate gestation weeks
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var totalDays = pregnancy.ExpectedBirthDate.DayNumber - today.DayNumber;
            var gestationWeeks = Math.Max(0, (280 - totalDays) / 7); // Assuming 280 days gestation
            
            result.Add(new PregnancyDto(
                pregnancy.AnimalId,
                animalTag,
                pregnancy.ExpectedBirthDate,
                gestationWeeks
            ));
        }
        
        return result;
    }
}

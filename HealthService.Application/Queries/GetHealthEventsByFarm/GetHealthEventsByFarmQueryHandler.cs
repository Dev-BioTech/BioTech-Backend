using MediatR;
using HealthService.Application.DTOs;
using HealthService.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthService.Application.Queries.GetHealthEventsByFarm;

public class GetHealthEventsByFarmQueryHandler : IRequestHandler<GetHealthEventsByFarmQuery, List<HealthEventResponse>>
{
    private readonly IHealthEventRepository _repository;

    public GetHealthEventsByFarmQueryHandler(IHealthEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<HealthEventResponse>> Handle(GetHealthEventsByFarmQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.GetByFarmIdAsync(request.FarmId, request.Page, request.PageSize, cancellationToken);

        return events.Select(e => new HealthEventResponse(
            e.Id,
            e.FarmId,
            e.EventDate,
            e.EventType,
            e.BatchId,
            e.AnimalId,
            e.DiseaseDiagnosisId,
            e.ProfessionalId,
            e.ServiceCost.Amount,
            e.Observations,
            e.CreatedAt,
            e.CreatedBy
        )).ToList();
    }
}

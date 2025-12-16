using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Interfaces;
using ReproductionService.Domain.Entities;

namespace ReproductionService.Application.Queries.GetReproductionEventsByType;

public class GetReproductionEventsByTypeQueryHandler : IRequestHandler<GetReproductionEventsByTypeQuery, ReproductionEventListResponse>
{
    private readonly IReproductionEventRepository _repository;

    public GetReproductionEventsByTypeQueryHandler(IReproductionEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReproductionEventListResponse> Handle(GetReproductionEventsByTypeQuery request, CancellationToken ct)
    {
        var pageSize = request.PageSize > 10 ? 10 : request.PageSize;
        var events = await _repository.GetByTypeAsync(request.Type, request.FarmId, request.Page, pageSize, ct);
        var responses = events.Select(MapToResponse).ToList();
        return new ReproductionEventListResponse(responses, responses.Count);
    }

    private static ReproductionEventResponse MapToResponse(ReproductionEvent entity)
    {
        return new ReproductionEventResponse(
            entity.Id,
            entity.FarmId,
            entity.EventDate,
            entity.AnimalId,
            entity.EventType,
            entity.Observations,
            entity.MaleAnimalId,
            entity.IsSuccessful,
            entity.ExpectedBirthDate,
            entity.OffspringCount,
            entity.IsCancelled,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.RegisteredBy
        );
    }
}

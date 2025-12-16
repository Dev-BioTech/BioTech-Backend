using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetHealthEventsByAnimalQuery(
    long AnimalId,
    string? EventType
) : IRequest<HealthEventListResponse>;

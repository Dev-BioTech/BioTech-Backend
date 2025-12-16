using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetHealthEventsByTypeQuery(
    string EventType,
    int FarmId,
    DateOnly? FromDate,
    DateOnly? ToDate
) : IRequest<HealthEventListResponse>;

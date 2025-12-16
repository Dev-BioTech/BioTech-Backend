using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetHealthEventsByFarmQuery(
    int FarmId,
    DateOnly? FromDate,
    DateOnly? ToDate,
    string? EventType
) : IRequest<HealthEventListResponse>;

using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetHealthEventsByBatchQuery(
    int BatchId,
    string? EventType
) : IRequest<HealthEventListResponse>;

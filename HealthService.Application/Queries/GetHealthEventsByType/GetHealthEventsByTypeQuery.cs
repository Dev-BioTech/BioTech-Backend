using MediatR;
using HealthService.Application.DTOs;
using System.Collections.Generic;

namespace HealthService.Application.Queries.GetHealthEventsByType;

public record GetHealthEventsByTypeQuery(string EventType, int FarmId, int Page = 1, int PageSize = 10) : IRequest<List<HealthEventResponse>>;

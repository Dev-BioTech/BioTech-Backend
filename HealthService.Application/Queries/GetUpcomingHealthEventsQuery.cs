using HealthService.Application.DTOs;
using MediatR;

namespace HealthService.Application.Queries;

public record GetUpcomingHealthEventsQuery(int Limit = 4) : IRequest<IEnumerable<UpcomingHealthEventDto>>;

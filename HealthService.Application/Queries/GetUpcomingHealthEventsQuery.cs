using HealthService.Application.DTOs;
using MediatR;

namespace HealthService.Application.Queries;

public record GetUpcomingHealthEventsQuery(int FarmId, int Limit = 10) : IRequest<IEnumerable<UpcomingHealthEventDto>>;

using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetRecentTreatmentsQuery(int FarmId, int Limit = 10) : IRequest<IEnumerable<HealthEventResponse>>;

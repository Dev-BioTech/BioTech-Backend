using MediatR;
using HealthService.Application.DTOs;

namespace HealthService.Application.Queries;

public record GetHealthDashboardStatsQuery(int FarmId) : IRequest<HealthDashboardStats>;

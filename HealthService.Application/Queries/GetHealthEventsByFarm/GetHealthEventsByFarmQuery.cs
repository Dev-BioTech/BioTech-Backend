using MediatR;
using HealthService.Application.DTOs;
using System.Collections.Generic;

namespace HealthService.Application.Queries.GetHealthEventsByFarm;

public record GetHealthEventsByFarmQuery(int FarmId, int Page = 1, int PageSize = 10) : IRequest<List<HealthEventResponse>>;

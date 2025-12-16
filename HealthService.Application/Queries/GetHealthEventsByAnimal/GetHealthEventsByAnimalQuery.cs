using MediatR;
using HealthService.Application.DTOs;
using System.Collections.Generic;

namespace HealthService.Application.Queries.GetHealthEventsByAnimal;

public record GetHealthEventsByAnimalQuery(long AnimalId, int Page = 1, int PageSize = 10) : IRequest<List<HealthEventResponse>>;

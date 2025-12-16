using MediatR;
using ReproductionService.Application.DTOs;

namespace ReproductionService.Application.Queries.GetReproductionEventsByAnimal;

public record GetReproductionEventsByAnimalQuery(long AnimalId) : IRequest<ReproductionEventListResponse>;

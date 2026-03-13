using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries.GetBreeds;

public record GetBreedsQuery() : IRequest<IEnumerable<BreedResponse>>;

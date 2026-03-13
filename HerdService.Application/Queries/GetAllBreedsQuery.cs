using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Queries;

public record GetAllBreedsQuery : IRequest<IEnumerable<BreedResponse>>;

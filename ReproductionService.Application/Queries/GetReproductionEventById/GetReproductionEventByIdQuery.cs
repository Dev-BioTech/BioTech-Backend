using MediatR;
using ReproductionService.Application.DTOs;

namespace ReproductionService.Application.Queries.GetReproductionEventById;

public record GetReproductionEventByIdQuery(long Id) : IRequest<ReproductionEventResponse>;

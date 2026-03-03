using ReproductionService.Application.DTOs;
using MediatR;

namespace ReproductionService.Application.Queries;

public record GetBirthsByFarmQuery(int FarmId) : IRequest<IEnumerable<BirthDto>>;

using ReproductionService.Application.DTOs;
using MediatR;

namespace ReproductionService.Application.Queries;

public record GetPregnanciesByFarmQuery(int FarmId) : IRequest<IEnumerable<PregnancyDto>>;

using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Queries;

public record GetPaddocksByFarmQuery(int FarmId) : IRequest<IEnumerable<PaddockResponse>>;

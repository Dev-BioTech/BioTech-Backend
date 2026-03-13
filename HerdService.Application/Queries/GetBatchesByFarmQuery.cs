using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Queries;

public record GetBatchesByFarmQuery(int FarmId) : IRequest<IEnumerable<BatchResponse>>;

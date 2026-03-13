using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries.GetBatchesByFarm;

public record GetBatchesByFarmQuery(int FarmId, bool IncludeInactive = false) : IRequest<IEnumerable<BatchResponse>>;

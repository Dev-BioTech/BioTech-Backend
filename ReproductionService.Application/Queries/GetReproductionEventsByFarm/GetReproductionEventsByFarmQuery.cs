using MediatR;
using ReproductionService.Application.DTOs;

namespace ReproductionService.Application.Queries.GetReproductionEventsByFarm;

public record GetReproductionEventsByFarmQuery(int FarmId, DateTime? FromDate, DateTime? ToDate) : IRequest<ReproductionEventListResponse>;

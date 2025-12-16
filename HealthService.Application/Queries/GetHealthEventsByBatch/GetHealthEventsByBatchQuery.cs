using MediatR;
using HealthService.Application.DTOs;
using System.Collections.Generic;

namespace HealthService.Application.Queries.GetHealthEventsByBatch;

public record GetHealthEventsByBatchQuery(int BatchId, int Page = 1, int PageSize = 10) : IRequest<List<HealthEventResponse>>;

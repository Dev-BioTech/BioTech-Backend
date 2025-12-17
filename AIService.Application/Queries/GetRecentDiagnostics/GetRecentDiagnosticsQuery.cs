using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Queries.GetRecentDiagnostics;

public record GetRecentDiagnosticsQuery(int Count = 20, string? ServiceName = null) : IRequest<IEnumerable<DiagnosticSession>>;

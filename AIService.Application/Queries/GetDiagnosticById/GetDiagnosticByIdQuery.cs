using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Queries.GetDiagnosticById;

public record GetDiagnosticByIdQuery(string SessionId) : IRequest<DiagnosticSession?>;

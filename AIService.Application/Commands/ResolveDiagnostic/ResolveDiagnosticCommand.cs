using MediatR;

namespace AIService.Application.Commands.ResolveDiagnostic;

public record ResolveDiagnosticCommand(string SessionId, string ResolutionNotes) : IRequest<bool>;

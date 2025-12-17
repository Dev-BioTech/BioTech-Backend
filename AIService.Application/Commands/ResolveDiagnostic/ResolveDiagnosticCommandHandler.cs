using AIService.Application.Interfaces;
using MediatR;

namespace AIService.Application.Commands.ResolveDiagnostic;

public class ResolveDiagnosticCommandHandler : IRequestHandler<ResolveDiagnosticCommand, bool>
{
    private readonly IDiagnosticSessionRepository _repository;

    public ResolveDiagnosticCommandHandler(IDiagnosticSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ResolveDiagnosticCommand command, CancellationToken cancellationToken)
    {
        var session = await _repository.GetBySessionIdAsync(command.SessionId);
        if (session == null) return false;
        
        session.IsResolved = true;
        session.ResolvedAt = DateTime.UtcNow;
        session.ResolutionNotes = command.ResolutionNotes;
        session.Status = "Resolved";
        
        await _repository.UpdateAsync(session);
        return true;
    }
}

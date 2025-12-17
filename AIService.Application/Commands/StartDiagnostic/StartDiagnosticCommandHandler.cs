using AIService.Application.DTOs;
using AIService.Application.Interfaces;
using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Commands.StartDiagnostic;

public class StartDiagnosticCommandHandler : IRequestHandler<StartDiagnosticCommand, DiagnosticResponse>
{
    private readonly IDiagnosticSessionRepository _repository;

    public StartDiagnosticCommandHandler(IDiagnosticSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiagnosticResponse> Handle(StartDiagnosticCommand command, CancellationToken cancellationToken)
    {
        var session = new DiagnosticSession
        {
            ServiceName = command.Request.ServiceName,
            Status = "InProgress",
            StartedAt = DateTime.UtcNow,
            ErrorCode = 0, // Placeholder
            CreatedAt = DateTime.UtcNow
        };
        
        await _repository.AddAsync(session);
        
        // In a real async scenario, we might trigger a background job here.
        // For now, we just create the session ticket.
        
        return new DiagnosticResponse(session.SessionId, session.Status);
    }
}

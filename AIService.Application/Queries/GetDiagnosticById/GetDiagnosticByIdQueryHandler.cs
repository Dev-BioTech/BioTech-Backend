using AIService.Application.Interfaces;
using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Queries.GetDiagnosticById;

public class GetDiagnosticByIdQueryHandler : IRequestHandler<GetDiagnosticByIdQuery, DiagnosticSession?>
{
    private readonly IDiagnosticSessionRepository _repository;

    public GetDiagnosticByIdQueryHandler(IDiagnosticSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<DiagnosticSession?> Handle(GetDiagnosticByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetBySessionIdAsync(request.SessionId);
    }
}

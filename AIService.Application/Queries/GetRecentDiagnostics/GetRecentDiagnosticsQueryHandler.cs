using AIService.Application.Interfaces;
using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Queries.GetRecentDiagnostics;

public class GetRecentDiagnosticsQueryHandler : IRequestHandler<GetRecentDiagnosticsQuery, IEnumerable<DiagnosticSession>>
{
    private readonly IDiagnosticSessionRepository _repository;

    public GetRecentDiagnosticsQueryHandler(IDiagnosticSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DiagnosticSession>> Handle(GetRecentDiagnosticsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetRecentAsync(request.Count, request.ServiceName);
    }
}

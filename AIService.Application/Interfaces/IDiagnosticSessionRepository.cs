using AIService.Domain.Entities;

namespace AIService.Application.Interfaces;

public interface IDiagnosticSessionRepository
{
    Task<DiagnosticSession> GetByIdAsync(long id);
    Task<DiagnosticSession> GetBySessionIdAsync(string sessionId);
    Task<IEnumerable<DiagnosticSession>> GetRecentAsync(int count, string? serviceName);
    Task AddAsync(DiagnosticSession session);
    Task UpdateAsync(DiagnosticSession session);
}

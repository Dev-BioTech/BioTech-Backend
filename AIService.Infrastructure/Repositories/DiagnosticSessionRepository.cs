using AIService.Application.Interfaces;
using AIService.Domain.Entities;
using AIService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIService.Infrastructure.Repositories;

public class DiagnosticSessionRepository : IDiagnosticSessionRepository
{
    private readonly DiagnosticDbContext _context;

    public DiagnosticSessionRepository(DiagnosticDbContext context)
    {
        _context = context;
    }

    public async Task<DiagnosticSession> GetByIdAsync(long id)
    {
        return await _context.DiagnosticSessions.FindAsync(id);
    }

    public async Task<DiagnosticSession> GetBySessionIdAsync(string sessionId)
    {
        return await _context.DiagnosticSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);
    }

    public async Task<IEnumerable<DiagnosticSession>> GetRecentAsync(int count, string? serviceName)
    {
        var query = _context.DiagnosticSessions.AsQueryable();
        
        if (!string.IsNullOrEmpty(serviceName))
        {
            query = query.Where(s => s.ServiceName == serviceName);
        }

        return await query.OrderByDescending(s => s.StartedAt).Take(count).ToListAsync();
    }

    public async Task AddAsync(DiagnosticSession session)
    {
        await _context.DiagnosticSessions.AddAsync(session);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DiagnosticSession session)
    {
        _context.DiagnosticSessions.Update(session);
        await _context.SaveChangesAsync();
    }
}

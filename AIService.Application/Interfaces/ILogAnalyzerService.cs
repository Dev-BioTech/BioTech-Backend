using AIService.Application.DTOs;

namespace AIService.Application.Interfaces;

public interface ILogAnalyzerService
{
    Task<LogAnalysisResult> AnalyzeLogsAsync(string serviceName, int lineCount = 100, CancellationToken ct = default);
}

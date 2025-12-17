using AIService.Application.DTOs;

namespace AIService.Application.Interfaces;

public interface IAiAnalysisService
{
    Task<AiDiagnosticResult> AnalyzeError502Async(
        string serviceName,
        string logSnapshot,
        ServiceHealthStatus healthStatus,
        DockerInspectionResult dockerInfo,
        CancellationToken ct = default);

    Task<List<string>> GenerateRecommendationsAsync(
        string rootCause,
        string serviceName,
        CancellationToken ct = default);
}

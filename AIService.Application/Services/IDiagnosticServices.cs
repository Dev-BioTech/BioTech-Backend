using AIService.Application.DTOs;

namespace AIService.Application.Services;

public interface ILogAnalyzerService
{
    Task<LogAnalysisResult> AnalyzeLogsAsync(string serviceName, int lineCount = 100, CancellationToken ct = default);
}

public interface IHealthCheckService
{
    Task<ServiceHealthStatus> CheckServiceHealthAsync(string serviceName, CancellationToken ct = default);
}

public interface IDockerInspectorService
{
    Task<DockerInspectionResult> InspectContainerAsync(string serviceName, CancellationToken ct = default);
}

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

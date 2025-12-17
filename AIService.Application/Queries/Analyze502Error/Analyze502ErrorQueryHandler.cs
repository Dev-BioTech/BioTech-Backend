using AIService.Application.DTOs;
using AIService.Application.Interfaces;
using AIService.Domain.Entities;
using MediatR;

namespace AIService.Application.Queries.Analyze502Error;

public class Analyze502ErrorQueryHandler : IRequestHandler<Analyze502ErrorQuery, Error502AnalysisResponse>
{
    private readonly IHealthCheckService _healthCheckService;
    private readonly IDockerInspectorService _dockerInspectorService;
    private readonly ILogAnalyzerService _logAnalyzerService;
    private readonly IAiAnalysisService _aiAnalysisService;
    private readonly IDiagnosticSessionRepository _diagnosticSessionRepository;

    public Analyze502ErrorQueryHandler(
        IHealthCheckService healthCheckService,
        IDockerInspectorService dockerInspectorService,
        ILogAnalyzerService logAnalyzerService,
        IAiAnalysisService aiAnalysisService,
        IDiagnosticSessionRepository diagnosticSessionRepository)
    {
        _healthCheckService = healthCheckService;
        _dockerInspectorService = dockerInspectorService;
        _logAnalyzerService = logAnalyzerService;
        _aiAnalysisService = aiAnalysisService;
        _diagnosticSessionRepository = diagnosticSessionRepository;
    }

    public async Task<Error502AnalysisResponse> Handle(Analyze502ErrorQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var serviceName = request.ServiceName;

        // 1. Health Check
        var healthStatus = await _healthCheckService.CheckServiceHealthAsync(serviceName, cancellationToken);

        // 2. Docker Inspection
        var dockerInfo = request.InspectDocker 
            ? await _dockerInspectorService.InspectContainerAsync(serviceName, cancellationToken)
            : new DockerInspectionResult(false, "Skipped", 0, 0, 0, "Unknown", 0, TimeSpan.Zero);

        // 3. Log Analysis
        var logAnalysis = request.IncludeLogs
            ? await _logAnalyzerService.AnalyzeLogsAsync(serviceName, 100, cancellationToken)
            : new LogAnalysisResult(0, 0, 0, new List<string>(), new List<string>(), "Skipped");

        // 4. AI Analysis
        var aiResult = await _aiAnalysisService.AnalyzeError502Async(
            serviceName,
            logAnalysis.LogSnapshot,
            healthStatus,
            dockerInfo,
            cancellationToken);

        // 5. Save Session
        var session = new DiagnosticSession
        {
            ServiceName = serviceName,
            ErrorCode = 502,
            ErrorMessage = healthStatus.ErrorMessage ?? "502 Bad Gateway",
            RequestUrl = request.RequestUrl,
            RequestMethod = request.RequestMethod,
            Status = "Completed",
            StartedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            
            LogSnapshot = logAnalysis.LogSnapshot,
            HealthCheckResult = $"Healthy: {healthStatus.IsHealthy}, Time: {healthStatus.ResponseTimeMs}ms",
            NetworkDiagnostic = dockerInfo.NetworkStatus,
            ResourceUsage = $"CPU: {dockerInfo.CpuUsagePercent}%, Mem: {dockerInfo.MemoryUsageMb}MB",
            
            AiAnalysis = aiResult.DetailedAnalysis,
            AiRecommendations = string.Join("|", aiResult.Recommendations),
            DetectedPattern = aiResult.RootCause,
            ConfidenceScore = aiResult.ConfidenceScore,
            
            CreatedAt = DateTime.UtcNow
        };

        await _diagnosticSessionRepository.AddAsync(session);

        return new Error502AnalysisResponse(
            SessionId: session.SessionId,
            ServiceName: serviceName,
            HealthStatus: healthStatus,
            LogAnalysis: logAnalysis,
            DockerStatus: dockerInfo,
            AiDiagnosis: aiResult,
            RecommendedActions: aiResult.Recommendations,
            ConfidenceScore: aiResult.ConfidenceScore
        );
    }
}

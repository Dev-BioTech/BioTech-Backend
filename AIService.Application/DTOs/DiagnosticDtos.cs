namespace AIService.Application.DTOs;

public record Analyze502Request(
    string ServiceName,
    string? RequestUrl,
    string? RequestMethod,
    bool IncludeLogs = true,
    bool RunHealthChecks = true,
    bool InspectDocker = true
);

public record Error502AnalysisResponse(
    string SessionId,
    string ServiceName,
    ServiceHealthStatus HealthStatus,
    LogAnalysisResult LogAnalysis,
    DockerInspectionResult DockerStatus,
    AiDiagnosticResult AiDiagnosis,
    List<string> RecommendedActions,
    int ConfidenceScore
);

public record ServiceHealthStatus(
    string ServiceName,
    bool IsHealthy,
    int ResponseTimeMs,
    string? ErrorMessage,
    DateTime LastChecked
);

public record LogAnalysisResult(
    int TotalLines,
    int ErrorCount,
    int WarningCount,
    List<string> RecentErrors,
    List<string> RecentExceptions,
    string LogSnapshot
);

public record DockerInspectionResult(
    bool IsRunning,
    string Status,
    double CpuUsagePercent,
    long MemoryUsageMb,
    long MemoryLimitMb,
    string NetworkStatus,
    int RestartCount,
    TimeSpan Uptime
);

public record AiDiagnosticResult(
    string RootCause,
    string DetailedAnalysis,
    List<string> PossibleCauses,
    List<string> Recommendations,
    int ConfidenceScore
);

public record StartDiagnosticRequest(string ServiceName);

public record DiagnosticResponse(string SessionId, string Status);

public record ResolveDiagnosticRequest(string ResolutionNotes);

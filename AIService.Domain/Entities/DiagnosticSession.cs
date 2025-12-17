using AIService.Domain.Common;

namespace AIService.Domain.Entities;

public class DiagnosticSession : IAuditableEntity
{
    public long Id { get; set; }
    
    // Session Information
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Status { get; set; } = "InProgress";  // "InProgress", "Completed", "Failed"
    
    // Error Context
    public string ServiceName { get; set; } = string.Empty;  // "FeedingService", "HerdService", etc.
    public int ErrorCode { get; set; }  // 502, 500, 503, etc.
    public string? ErrorMessage { get; set; }
    public string? RequestUrl { get; set; }
    public string? RequestMethod { get; set; }
    
    // Diagnostic Data
    public string? LogSnapshot { get; set; }  // Last 100 lines of logs
    public string? HealthCheckResult { get; set; }
    public string? NetworkDiagnostic { get; set; }
    public string? ResourceUsage { get; set; }
    
    // AI Analysis
    public string? AiAnalysis { get; set; }  // AI-generated root cause analysis
    public string? AiRecommendations { get; set; }  // AI-generated fix recommendations
    public string? DetectedPattern { get; set; }  // "Database Timeout", "Service Down", etc.
    public int ConfidenceScore { get; set; }  // 0-100
    
    // Resolution
    public bool IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}

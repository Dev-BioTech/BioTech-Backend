using AIService.Domain.Common;

namespace AIService.Domain.Entities;

public class ErrorPattern : IAuditableEntity
{
    public int Id { get; set; }
    public string PatternName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;  // JSON array of symptoms
    public string CommonCauses { get; set; } = string.Empty;  // JSON array
    public string RecommendedFixes { get; set; } = string.Empty;  // JSON array
    public int OccurrenceCount { get; set; }
    public DateTime LastOccurrence { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}

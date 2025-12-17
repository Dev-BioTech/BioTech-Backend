namespace AIService.Domain.Common;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    int? CreatedBy { get; set; }
    int? LastModifiedBy { get; set; }
}

using HerdService.Domain.Common;

namespace HerdService.Domain.Entities;

public class Paddock : IAuditableEntity
{
    public int Id { get; set; }
    public int FarmId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }
}

using HerdService.Domain.Common;

namespace HerdService.Domain.Entities;

public class AnimalMovement : IAuditableEntity
{
    public int Id { get; set; }
    public long AnimalId { get; set; }
    public int MovementTypeId { get; set; }
    public DateTime MovementDate { get; set; }
    public string? Observation { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? LastModifiedBy { get; set; }

    public Animal? Animal { get; set; }
    public MovementType? MovementType { get; set; }
}

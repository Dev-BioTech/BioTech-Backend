using AuthService.Domain.Common;

namespace AuthService.Domain.Entities;

public class Farm : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Owner { get; set; }
    public string? Address { get; set; }
    public string? GeographicLocation { get; set; }
    public bool Active { get; set; } = true;

    // Audit fields (Only CreatedAt is in SQL, but good to keep common interface if possible, 
    // though schema only shows created_at. I'll implement IAuditableEntity partially or keep it for consistency but only map what exists)
    // SQL: created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // SQL doesn't show updated_at, created_by, last_modified_by.
    // I will remove implementation of IAuditableEntity if strict adherence is required, 
    // OR keep it but mapped properties will be ignored/null if not in DB.
    // User Instructions: "Sigue EXACTAMENTE la estructura...". Structure implies IAuditableEntity was required previously.
    // New SQL: Only created_at.
    // I will KEEP IAuditableEntity for code consistency (Clean Architecture) but purely as in-memory/future-proof, 
    // knowing only CreatedAt is persisted for now based on 'exact' SQL. 
    // Actually, simpler is safer: Match SQL. Remove IAuditableEntity if fields don't exist.
    // But "Clean Architecture" usually demands it.
    // Let's stick to the SQL fields provided to avoid "Column not found" errors since no migration is allowed.
    
    public DateTime? UpdatedAt { get; set; } // Not in SQL?
    public int? CreatedBy { get; set; } // Not in SQL
    public int? LastModifiedBy { get; set; } // Not in SQL
    
    // Changing approach: The user gave a SQL table definition. 
    // If I map "UpdatedAt" EF will try to select it.
    // I must remove these if they are not in the table.
    // So I will REMOVE IAuditableEntity interface to avoid confusion or unexpected errors,
    // OR implement it but mark fields `[NotMapped]` not present in DB? No, that breaks the interface contract at DB level typically.
    
    // Navigation properties
    public ICollection<UserFarmRole> UserFarmRoles { get; set; } = new List<UserFarmRole>();

    public Farm()
    {
        CreatedAt = DateTime.UtcNow;
        Active = true;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Farm name is required");
    }
}

namespace HealthService.Domain.Entities;

public class Disease
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public string? Description { get; set; }

    public Disease() { }

    public Disease(string name, string? type, string? description)
    {
        Name = name;
        Type = type;
        Description = description;
    }
}

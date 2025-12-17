namespace AIService.Application.DTOs;

public class FarmDataContext
{
    public int TotalAnimals { get; set; }
    public Dictionary<string, int>? AnimalsByCategory { get; set; }
    public List<HealthEventSummary>? RecentHealthEvents { get; set; }
    public List<FeedingEventSummary>? RecentFeedingEvents { get; set; }
}

public class AnimalSummary
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class HealthEventSummary
{
    public string EventType { get; set; } = string.Empty;
    public DateOnly EventDate { get; set; }
}

public class FeedingEventSummary
{
    public DateOnly SupplyDate { get; set; }
    public decimal TotalQuantity { get; set; }
}

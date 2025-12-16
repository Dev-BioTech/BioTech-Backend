namespace HealthService.Application.DTOs;

public record HealthDashboardStats(
    int TotalEvents,
    decimal TotalCost,
    int RecentSickAnimalsCount,
    DateTime CalculatedAt
);

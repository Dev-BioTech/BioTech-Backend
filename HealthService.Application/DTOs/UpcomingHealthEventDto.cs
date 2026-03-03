namespace HealthService.Application.DTOs;

public record UpcomingHealthEventDto(
    long Id,
    string AnimalTag,
    string EventType,
    DateOnly ScheduledDate
);

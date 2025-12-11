namespace FeedingService.Application.DTOs;

public record FeedingEventListResponse(
    IEnumerable<FeedingEventResponse> FeedingEvents,
    int TotalCount
);
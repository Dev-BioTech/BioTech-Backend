using FeedingService.Application.DTOs;
using MediatR;

namespace FeedingService.Application.Commands.CancelFeedingEvent;

public record CancelFeedingEventCommand(long Id) : IRequest<FeedingEventResponse>;

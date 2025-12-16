using FeedingService.Application.DTOs;
using MediatR;

namespace FeedingService.Application.Queries.GetFeedingEventsByProduct;

public record GetFeedingEventsByProductQuery(int ProductId) : IRequest<FeedingEventListResponse>;

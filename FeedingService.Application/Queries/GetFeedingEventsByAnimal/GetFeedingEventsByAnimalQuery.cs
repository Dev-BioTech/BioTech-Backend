using FeedingService.Application.DTOs;
using MediatR;

namespace FeedingService.Application.Queries.GetFeedingEventsByAnimal;

public record GetFeedingEventsByAnimalQuery(long AnimalId) : IRequest<FeedingEventListResponse>;

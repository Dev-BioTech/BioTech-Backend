using FeedingService.Application.DTOs;
using MediatR;

namespace FeedingService.Application.Commands.RecalculateTotalCost;

public record RecalculateTotalCostCommand(long Id) : IRequest<FeedingEventResponse>;

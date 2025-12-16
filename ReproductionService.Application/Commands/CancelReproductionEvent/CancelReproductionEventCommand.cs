using MediatR;
using ReproductionService.Application.DTOs;

namespace ReproductionService.Application.Commands.CancelReproductionEvent;

public record CancelReproductionEventCommand(long Id) : IRequest<ReproductionEventResponse>;

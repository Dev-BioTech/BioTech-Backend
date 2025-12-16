using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.Commands.CreateReproductionEvent;

public record CreateReproductionEventCommand(
    int FarmId,
    DateTime EventDate,
    long AnimalId,
    ReproductionEventType EventType,
    string? Observations,
    int? MaleAnimalId,
    int? SemenBatchId,
    bool? PregnancyResult,
    int? OffspringCount,
    int? RegisteredBy
) : IRequest<ReproductionEventResponse>;

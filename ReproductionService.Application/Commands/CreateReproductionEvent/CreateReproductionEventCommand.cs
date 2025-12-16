using MediatR;
using ReproductionService.Application.DTOs;
using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.Commands.CreateReproductionEvent;

public record CreateReproductionEventCommand(
    int FarmId,
    long AnimalId,
    ReproductionEventType EventType,
    DateTime EventDate,
    int? RegisteredBy,
    string? Observations,
    int? MaleAnimalId,
    // int? SemenBatchId, // Removed
    bool? IsSuccessful, // Was PregnancyResult
    DateOnly? ExpectedBirthDate, // Added
    int? OffspringCount
) : IRequest<ReproductionEventResponse>;

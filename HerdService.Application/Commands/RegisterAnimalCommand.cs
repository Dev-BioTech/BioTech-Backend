using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Commands;

public record RegisterAnimalCommand(
    string VisualCode,
    int FarmId,
    string Sex,
    DateOnly BirthDate,
    int? CategoryId,
    int? BreedId,
    string? Name,
    string? ElectronicCode,
    string? Color,
    string? Purpose,
    string? Origin,
    decimal InitialCost,
    long? MotherId,
    long? FatherId,
    string? ExternalMother,
    string? ExternalFather,
    int? BatchId,
    int? PaddockId,
    decimal? Weight,
    decimal? Height,
    int? UserId // From Gateway
) : IRequest<AnimalResponse>;

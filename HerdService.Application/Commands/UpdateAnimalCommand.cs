using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Commands;

public record UpdateAnimalCommand(
    long Id,
    string VisualCode,
    string? ElectronicCode,
    string? Name,
    string? Color,
    int? BreedId,
    int? CategoryId,
    string? Purpose,
    string? Sex,
    DateOnly? BirthDate,
    string? Origin,
    DateOnly? EntryDate,
    decimal? InitialCost,
    long? MotherId,
    long? FatherId,
    string? ExternalMother,
    string? ExternalFather,
    int? UserId
) : IRequest<AnimalResponse>;

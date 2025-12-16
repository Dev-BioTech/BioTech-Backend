using HerdService.Domain.Enums;

namespace HerdService.Application.DTOs;

public record AnimalResponse(
    long Id,
    int FarmId,
    string Identifier,
    DateTime BirthDate,
    int BreedId,
    int? CategoryId,
    int? BatchId,
    int? PaddockId,
    int Gender,
    int Status,
    DateTime CreatedAt,
    int? CreatedBy
);

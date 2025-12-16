using MediatR;
using HerdService.Application.DTOs;
using HerdService.Domain.Enums;

namespace HerdService.Application.Commands.CreateAnimal;

public record CreateAnimalCommand(
    int FarmId,
    string Identifier,
    DateTime BirthDate,
    int BreedId,
    int? CategoryId,
    int? BatchId,
    int? PaddockId,
    int Gender,
    int Status,
    int RegisteredBy
) : IRequest<AnimalResponse>;

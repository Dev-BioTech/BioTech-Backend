using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.CreateBreed;

public record CreateBreedCommand(string Name) : IRequest<BreedResponse>;

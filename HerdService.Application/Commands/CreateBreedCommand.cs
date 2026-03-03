using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands;

public record CreateBreedCommand(string Name) : IRequest<BreedResponse>;

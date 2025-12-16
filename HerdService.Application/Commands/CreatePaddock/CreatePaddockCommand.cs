using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.CreatePaddock;

public record CreatePaddockCommand(string Name, int FarmId) : IRequest<PaddockResponse>;

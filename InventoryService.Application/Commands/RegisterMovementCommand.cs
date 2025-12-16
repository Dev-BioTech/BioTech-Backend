using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Commands;

public record RegisterMovementCommand(RegisterMovementDto Dto) : IRequest<long>;

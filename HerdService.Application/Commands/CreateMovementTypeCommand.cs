using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Commands;

public record CreateMovementTypeCommand(
    string Name,
    string? Description,
    bool AffectsInventory,
    int InventorySign
) : IRequest<MovementTypeResponse>;

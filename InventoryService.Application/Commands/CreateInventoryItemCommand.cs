using MediatR;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Commands;

public record CreateInventoryItemCommand(
    string Name,
    decimal Quantity,
    string Unit,
    int FarmId = 0
) : IRequest<InventoryItemDto>;

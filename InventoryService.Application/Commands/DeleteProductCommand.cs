using MediatR;

namespace InventoryService.Application.Commands;

public record DeleteProductCommand(int Id) : IRequest;

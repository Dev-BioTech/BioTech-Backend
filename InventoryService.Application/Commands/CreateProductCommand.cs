using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Commands;

public record CreateProductCommand(CreateProductDto Dto) : IRequest<long>;

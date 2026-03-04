using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Commands;

public record UpdateProductCommand(int Id, UpdateProductDto Dto) : IRequest<ProductDto>;

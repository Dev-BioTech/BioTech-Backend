using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Queries;

public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

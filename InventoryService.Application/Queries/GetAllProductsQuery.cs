using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Queries;

public record GetAllProductsQuery(int FarmId) : IRequest<IEnumerable<ProductDto>>;

using InventoryService.Application.DTOs;
using MediatR;

namespace InventoryService.Application.Queries;

public record GetLowStockProductsQuery(int FarmId) : IRequest<IEnumerable<LowStockProductDto>>;

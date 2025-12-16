using InventoryService.Application.DTOs;
using InventoryService.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService.Application.Queries;


public record GetAllProductsQuery(int FarmId) : IRequest<IEnumerable<ProductDto>>;
public record GetLowStockProductsQuery(int FarmId) : IRequest<IEnumerable<ProductDto>>;

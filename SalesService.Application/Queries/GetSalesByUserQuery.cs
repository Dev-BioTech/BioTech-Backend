using SalesService.Application.DTOs;
using MediatR;

namespace SalesService.Application.Queries;

public record GetSalesByUserQuery : IRequest<IEnumerable<SaleDto>>;

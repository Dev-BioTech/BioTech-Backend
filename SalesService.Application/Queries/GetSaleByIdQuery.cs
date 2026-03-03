using SalesService.Application.DTOs;
using MediatR;

namespace SalesService.Application.Queries;

public record GetSaleByIdQuery(int Id) : IRequest<SaleDto?>;

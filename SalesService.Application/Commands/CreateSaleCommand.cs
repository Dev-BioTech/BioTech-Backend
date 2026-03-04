using SalesService.Application.DTOs;
using MediatR;

namespace SalesService.Application.Commands;

public record CreateSaleCommand(CreateSaleDto Dto) : IRequest<SaleDto>;

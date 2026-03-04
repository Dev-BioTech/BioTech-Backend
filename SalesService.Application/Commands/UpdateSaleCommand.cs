using SalesService.Application.DTOs;
using MediatR;

namespace SalesService.Application.Commands;

public record UpdateSaleCommand(int Id, UpdateSaleDto Dto) : IRequest<SaleDto>;

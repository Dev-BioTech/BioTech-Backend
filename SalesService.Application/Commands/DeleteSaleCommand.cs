using MediatR;

namespace SalesService.Application.Commands;

public record DeleteSaleCommand(int Id) : IRequest;

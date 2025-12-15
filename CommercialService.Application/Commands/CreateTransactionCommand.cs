using MediatR;
using CommercialService.Application.DTOs;

namespace CommercialService.Application.Commands;

public record CreateTransactionCommand(CreateTransactionDto Dto, int UserId) : IRequest<long>;

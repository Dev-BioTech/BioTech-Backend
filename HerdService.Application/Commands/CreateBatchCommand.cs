using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands;

public record CreateBatchCommand(string Name, int FarmId) : IRequest<BatchResponse>;

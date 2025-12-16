using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.CreateBatch;

public record CreateBatchCommand(string Name, int FarmId) : IRequest<BatchResponse>;

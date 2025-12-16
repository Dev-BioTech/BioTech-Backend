using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record CreateFarmCommand(CreateFarmDto FarmDto) : IRequest<FarmResponseDto>;

public record DeleteFarmCommand(int Id) : IRequest<bool>;

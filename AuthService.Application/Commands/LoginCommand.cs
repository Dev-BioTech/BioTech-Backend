using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record LoginCommand(LoginDto LoginDto) : IRequest<AuthResponseDto>;

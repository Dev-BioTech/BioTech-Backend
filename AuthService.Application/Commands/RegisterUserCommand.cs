using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record RegisterUserCommand(RegisterUserDto RegisterDto) : IRequest<int>;

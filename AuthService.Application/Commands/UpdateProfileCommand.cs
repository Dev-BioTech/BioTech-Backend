using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record UpdateProfileCommand(int UserId, UpdateProfileDto Dto) : IRequest;

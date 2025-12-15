using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Queries;

public record GetProfileQuery(int UserId) : IRequest<UserProfileDto>;

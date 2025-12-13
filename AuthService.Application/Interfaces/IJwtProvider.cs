using AuthService.Application.DTOs;
using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface IJwtProvider
{
    AuthResponseDto Generate(User user);
}

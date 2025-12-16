using MediatR;
using AuthService.Application.DTOs;

namespace AuthService.Application.Commands;

public record CreateTenantCommand(CreateTenantDto TenantDto) : IRequest<TenantResponseDto>;

// Add Update/Delete commands if needed, for now starting with Create

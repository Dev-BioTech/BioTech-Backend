using HealthService.Application.DTOs;
using MediatR;

namespace HealthService.Application.Commands;

public record UpdateHealthEventCommand(long Id, UpdateHealthEventDto Dto) : IRequest<HealthEventResponse>;

using MediatR;

namespace AuthService.Application.Commands.DeleteFarm;

public record DeleteFarmCommand(int Id, int? UserId) : IRequest<bool>;

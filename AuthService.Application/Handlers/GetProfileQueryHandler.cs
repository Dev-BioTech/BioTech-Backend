using MediatR;
using AuthService.Application.Queries;
using AuthService.Application.DTOs;
using AuthService.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace AuthService.Application.Handlers;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUserRepository _userRepository;

    public GetProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found");
        }

        return new UserProfileDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Active = user.Active,
            Roles = user.UserFarmRoles.Select(ufr => ufr.Role.Name).Distinct().ToList()
        };
    }
}

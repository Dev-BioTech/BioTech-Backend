using MediatR;
using AuthService.Application.Commands;
using AuthService.Domain.Interfaces;
using AuthService.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace AuthService.Application.Handlers;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UpdateProfileCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.FullName))
        {
            user.FullName = request.Dto.FullName;
        }

        if (!string.IsNullOrWhiteSpace(request.Dto.Password))
        {
            user.PasswordHash = _passwordHasher.Hash(request.Dto.Password);
        }

        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}

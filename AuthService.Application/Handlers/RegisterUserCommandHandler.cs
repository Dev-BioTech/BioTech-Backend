using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AuthService.Application.Commands;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(request.RegisterDto.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            FullName = request.RegisterDto.FullName,
            Email = request.RegisterDto.Email,
            PasswordHash = _passwordHasher.Hash(request.RegisterDto.Password),
            Active = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);

        return user.Id;
    }
}

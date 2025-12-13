using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Services;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResponseDto Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullName", user.FullName)
        };

        // Add Farm Roles to claims
        foreach (var userFarmRole in user.UserFarmRoles)
        {
            claims.Add(new Claim("farm_role", $"{userFarmRole.FarmId}:{userFarmRole.Role.Name}"));
        }

        var secret = _configuration["Jwt:Secret"];
        var expiration = _configuration["Jwt:ExpirationInMinutes"];

        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(expiration))
        {
            throw new InvalidOperationException("JWT Configuration is missing (Secret or Expiration).");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddMinutes(double.Parse(expiration));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto
        {
            Token = tokenString,
            Expiration = expiry,
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}

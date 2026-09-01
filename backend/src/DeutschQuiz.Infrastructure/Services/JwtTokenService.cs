using DeutschQuiz.Application;
using DeutschQuiz.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeutschQuiz.Infrastructure.Services;

public sealed class JwtTokenService(IConfiguration configuration)
{
    public AuthResult CreateToken(UserEntity user)
    {
        var signingKey = configuration["Jwt:SigningKey"];

        if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32)
        {
            throw new InvalidOperationException(
                "Jwt:SigningKey must be configured with at least 32 characters.");
        }

        var issuer = configuration["Jwt:Issuer"] ?? "DeutschQuiz.Api";
        var audience = configuration["Jwt:Audience"] ?? "DeutschQuiz.Web";
        var expiresAtUtc = DateTime.UtcNow.AddHours(2);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.DisplayName)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResult(
            new JwtSecurityTokenHandler().WriteToken(token),
            expiresAtUtc,
            new AuthUser(user.Id, user.Email, user.DisplayName));
    }
}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Infrastructure;

/// <summary>
/// JWT provider
/// </summary>
public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{ 
    private readonly JwtOptions _jwtOptions = jwtOptions?.Value 
        ?? throw new ArgumentNullException(nameof(JwtOptions));

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
        
        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature);
        
        Claim[] claims = 
            [
                new (ClaimTypes.NameIdentifier , user.Login)
            ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpiryHours),
            Audience = _jwtOptions.Audience,
            Issuer = _jwtOptions.Issuer,
            SigningCredentials = signingCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}
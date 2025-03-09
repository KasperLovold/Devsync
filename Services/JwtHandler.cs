using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevSync.Interfaces;
using DevSync.Interfaces.Handlers;
using DevSync.Models;
using Microsoft.IdentityModel.Tokens;

namespace DevSync.Services;

public class JwtHandler : IJwtHandler
{
    private readonly IConfiguration _configuration;

    public JwtHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public JwtSecurityToken? GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        if (user.Email == null || user.UserName == null) return null;
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        return new(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

    }

    public string GenerateKey(JwtSecurityToken token) => new JwtSecurityTokenHandler().WriteToken(token);
}
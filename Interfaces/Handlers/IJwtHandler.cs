using System.IdentityModel.Tokens.Jwt;
using DevSync.Models;

namespace DevSync.Interfaces.Handlers;

public interface IJwtHandler
{
   public JwtSecurityToken? GenerateToken(User user);
   public string GenerateKey(JwtSecurityToken token);
}
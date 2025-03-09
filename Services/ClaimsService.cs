using System.Security.Claims;
using DevSync.Interfaces;
using DevSync.Interfaces.Services;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DevSync.Services;

public class ClaimsService(IHttpContextAccessor httpContextAccessor) : IClaimsService
{
    public int? GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userId, out var id) ? id : null;
    }
}
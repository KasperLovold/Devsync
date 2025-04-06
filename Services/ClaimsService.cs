using System.Security.Claims;
using DevSync.Interfaces.Services;
using DevSync.Models.Enums;

namespace DevSync.Services;

public class ClaimsService(IHttpContextAccessor httpContextAccessor) : IClaimsService
{
    public int? GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userId, out var id) ? id : null;
    }

    public ClientType GetClientType()
    {
        var clientType = httpContextAccessor.HttpContext?.Request.Headers["X-Client-Type"].ToString();
        return clientType switch
        {
            "web" => ClientType.Web,
            "mobile" => ClientType.Mobile,
            _ => ClientType.Unknown
        };
    }
}

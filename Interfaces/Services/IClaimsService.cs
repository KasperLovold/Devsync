using DevSync.Models.Enums;

namespace DevSync.Interfaces.Services;

public interface IClaimsService
{
    int? GetUserId();
    ClientType GetClientType();
}

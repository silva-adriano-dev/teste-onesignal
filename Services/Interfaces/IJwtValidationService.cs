using System.Security.Claims;

namespace TrackingRealtime.Services.Interfaces;

public interface IJwtValidationService
{
    ClaimsPrincipal? ValidateToken(string token);
}
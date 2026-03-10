using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrackingRealtime.Services.Interfaces;

namespace TrackingRealtime.Services;

public class JwtValidationService : IJwtValidationService
{
    private readonly IConfiguration _config;

    public JwtValidationService(IConfiguration config)
    {
        _config = config;
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(
            _config["Jwt:SecretKey"]
        );

        try
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(
                token,
                parameters,
                out SecurityToken validatedToken
            );

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
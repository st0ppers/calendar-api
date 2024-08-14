using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CalendarApi.Contracts.Response;
using Microsoft.IdentityModel.Tokens;

namespace CalendarApi.Internal;

public static class TokenEndpoint
{
    public static TokenResponse Connect(JwtOptions jwtOptions, PlayerResponse player)
    {
        var tokenExpiration = TimeSpan.FromSeconds(jwtOptions.ExpirationSeconds);
        var accessToken = CreateAccessToken(
            jwtOptions,
            player.Username,
            tokenExpiration,
            new[] { "UpdateFreeTime" });

        return new()
        {
            Expiration = (int)tokenExpiration.TotalSeconds,
            AccessToken = accessToken,
        };
    }

    static string CreateAccessToken(
        JwtOptions jwtOptions,
        string username,
        TimeSpan expiration,
        string[] permissions)
    {
        var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);
        var symmetricKey = new SymmetricSecurityKey(keyBytes);

        var signingCredentials = new SigningCredentials(
            symmetricKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new("username", username),
            new("audience", jwtOptions.Audience)
        };

        var roleClaims = permissions.Select(x => new Claim("role", x));
        claims.AddRange(roleClaims);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.Add(expiration),
            signingCredentials: signingCredentials);

        var rawToken = new JwtSecurityTokenHandler().WriteToken(token);
        return rawToken;
    }
}

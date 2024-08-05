using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CalendarApi.Internal;

public static class TokenEndpoint
{
    public static IResult Connect(JwtOptions jwtOptions)
    {
        //TODO: implement the logic for username
        var userName = "a";

        var tokenExpiration = TimeSpan.FromSeconds(jwtOptions.ExpirationSeconds);
        var accessToken = CreateAccessToken(
            jwtOptions,
            userName,
            tokenExpiration,
            new[] { "UpdateFreeTime" });

        return Results.Ok(new
        {
            access_token = accessToken,
            expiration = (int)tokenExpiration.TotalSeconds,
            type = "bearer"
        });
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
             new Claim("username", username),
             new Claim("audience", jwtOptions.Audience)
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

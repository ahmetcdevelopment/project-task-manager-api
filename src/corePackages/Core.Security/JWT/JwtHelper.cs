using System.Collections.Immutable;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using NArchitecture.Core.Security.Constants;

namespace Core.Security.JWT;
public class JwtHelper<TUserId, TOperationClaimId> : ITokenHelper<TUserId, TOperationClaimId>
{
    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User<TUserId> user, SigningCredentials signingCredentials, IList<OperationClaim<TOperationClaimId>> operationClaims)
    {
        var jwt = new JwtSecurityToken(
            issuer: tokenOptions.Issuer,
            audience: tokenOptions.Audience,
            expires: DateTime.Now.AddMonths(tokenOptions.AccessTokenExpiration),
            notBefore: DateTime.Now,
            claims: SetClaims(user, operationClaims),
            signingCredentials: signingCredentials
        );
        return jwt;
    }

    public RefreshToken<TUserId> CreateRefreshToken(User<TUserId> user, string ipAddress)
    {
        var refreshToken = new RefreshToken<TUserId>
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresDate = DateTime.UtcNow.AddDays(7), // Changed Expires to ExpiresDate
            CreatedDate = DateTime.UtcNow,
            CreatedByIp = ipAddress,
            UserId = user.Id
        };
        return refreshToken;
    }

    public AccessToken CreateToken(User<TUserId> user, IList<OperationClaim<TOperationClaimId>> operationClaims)
    {
        var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        var securityKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var jwt = CreateJwtSecurityToken(tokenOptions, user, signingCredentials, operationClaims);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);


        return new AccessToken
        {
            Token = token,
            //token'ı bir ay süreli olarak değiştirdim.
            ExpirationDate = DateTime.Now.AddMonths(tokenOptions.AccessTokenExpiration)
        };
    }


    protected virtual IEnumerable<Claim> SetClaims(User<TUserId> user, IList<OperationClaim<TOperationClaimId>> operationClaims)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(CustomClaimTypes.TenantId, user.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(CustomClaimTypes.FirstName, user.FirstName),
            new Claim(CustomClaimTypes.LastName, user.LastName),
            new Claim(CustomClaimTypes.TenantName, user.TenantName),
        };

        claims.AddRange(operationClaims.Select(c => new Claim(ClaimTypes.Role, c.Name)));

        return claims;
    }
}

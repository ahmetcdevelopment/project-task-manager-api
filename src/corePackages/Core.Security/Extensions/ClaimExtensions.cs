using System.Security.Claims;

namespace Core.Security.Extensions;

public static class ClaimExtensions
{
    public static void AddEmail(this ICollection<Claim> claims, string email)
    {
        claims.Add(new Claim(ClaimTypes.Email, email));
    }
    public static void AddPhoto(this ICollection<Claim> claims, string photo)
    {
        claims.Add(new Claim("ProfilePhoto", photo));
    }
    public static void AddName(this ICollection<Claim> claims, string name)
    {
        claims.Add(new Claim(ClaimTypes.Name, name));
    }
    public static void AddGuid(this ICollection<Claim> claims, string guid)
    {
        claims.Add(new Claim("Guid", guid));
    }
    public static void AddTenantId(this ICollection<Claim> claims, int tenantId)
    {
        claims.Add(new Claim("TenantId", tenantId.ToString()));
    }
    public static void AddTenantName(this ICollection<Claim> claims, string? tenantName)
    {
        claims.Add(new Claim("TenantName", tenantName ?? string.Empty));
    }
    public static void AddClaimValues(this ICollection<Claim> claims, string key, string value)
    {
        claims.Add(new Claim(key, value));
    }
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
    }

    public static void AddRoles(this ICollection<Claim> claims, ICollection<string> roles)
    {
        foreach (string role in roles)
            claims.AddRole(role);
    }

    public static void AddRole(this ICollection<Claim> claims, string role)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }
}

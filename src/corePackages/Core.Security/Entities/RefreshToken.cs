using Core.Persistence.Repositories;

namespace Core.Security.Entities;
public class RefreshToken<TUserId> : Entity<TUserId>
{
    public RefreshToken() { }
    public RefreshToken(TUserId userId, string token, DateTime expiresDate, string createdByIp)
    {
        UserId = userId;
        Token = token;
        ExpiresDate = expiresDate;
        CreatedByIp = createdByIp;
    }
    public RefreshToken(TUserId id, TUserId userId, string token, DateTime expiresDate, string createdByIp)
    {
        Id = id;
        UserId = userId;
        Token = token;
        ExpiresDate = expiresDate;
        CreatedByIp = createdByIp;
    }

    public TUserId UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresDate { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? RevokedDate { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
}

using Core.Persistence.Repositories;
using Core.Security.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Security.Entities;

public class User<TId> : Entity<TId>
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public int? VerifyCode { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[]? ProfilePhoto { get; set; }

    public AuthenticatorType AuthenticatorType { get; set; }
    public int? TenantId { get; set; }
    [NotMapped]
    public string? TenantName { get; set; }
    public User()
    {
        Email = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        PasswordHash = Array.Empty<byte>();
        PasswordSalt = Array.Empty<byte>();
    }

    public User(int roleId, string email, string name, string lastName, byte[] passwordSalt, byte[] passwordHash, AuthenticatorType authenticatorType)
    {
        Email = email;
        FirstName = name;
        LastName = lastName;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;
        AuthenticatorType = authenticatorType;
    }

    public User(TId id, string email, string name, string lastName, byte[] passwordSalt, byte[] passwordHash, AuthenticatorType authenticatorType)
        : base(id)
    {
        Email = email;
        FirstName = name;
        LastName = lastName;
        Email = email;
        PasswordSalt = passwordSalt;
        PasswordHash = passwordHash;        
        AuthenticatorType = authenticatorType;
    }
}

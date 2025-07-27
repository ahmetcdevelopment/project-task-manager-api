
using System.Text.Json.Serialization;

namespace Core.Application.Dtos;


public class UserForLoginDto : IDto
{
    public Dictionary<string, string>? ClaimValues { get; set; }
    public string? Code { get; set; }
    public string? AuthenticatorCode { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public UserForLoginDto()
    {
        Code = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public UserForLoginDto(string kurumGuid, string email, string password)
    {
        Code = kurumGuid;
        Email = email;
        Password = password;
    }
}

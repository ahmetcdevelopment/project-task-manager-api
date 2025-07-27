namespace Core.Sms.Dtos;
public class VerificationStorageDto
{
    public string hash { get; set; } = default!;
    public DateTime expirationDate { get; set; }

    public string? operation { get; set; }
}

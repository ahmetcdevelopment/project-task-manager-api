namespace Core.Sms.Dtos;
public class SmsServiceConfig
{
    public Dictionary<string, SmsProviderSettings> SmsServices { get; set; }
    public SmsConfiguration SmsConfiguration { get; set; }
}

public class SmsProviderSettings
{
    public string? ApiId { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiUrl { get; set; }

    public string? OTP { get; set; }

    public string? OTN { get; set; }

    public string? NTN { get; set; }

    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region { get; set; }
    public string SenderId { get; set; }
}

public class SmsConfiguration
{
    public Dictionary<string, string>? Messages { get; set; }
    public string? Sender { get; set; }
}

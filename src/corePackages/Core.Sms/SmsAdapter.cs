using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Core.Sms.Dtos;
using Core.Sms.Enums;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Sms;


public class SmsAdapter
{
    // HttpClient tek örnek olarak yeniden kullanılıyor
    private   readonly HttpClient _httpClient = new HttpClient();
    private   readonly ConcurrentDictionary<string, string> _verificationStore = new();

    private   readonly SmsServiceConfig _config;

    private readonly AmazonSimpleNotificationServiceClient _snsClient;

    public SmsAdapter(
          SmsServiceConfig config
    )
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));

        var awsCredentials = new BasicAWSCredentials(
            _config.SmsServices[SmsServices.AMAZON.ToString()].AccessKey,
            _config.SmsServices[SmsServices.AMAZON.ToString()].SecretKey);

        _snsClient = new AmazonSimpleNotificationServiceClient(awsCredentials, RegionEndpoint.GetBySystemName(_config.SmsServices["Amazon"].Region));
    }

    //public   async Task<string> SendSmsAsync(
    //    SmsServices provider,
    //    List<string> phoneNumbers,
    //    List<string>? messages = null,
    //    SmsMessages? messageKey = SmsMessages.Custom,
    //    string? customMessage = null,
    //    SmsKeywordsDto? keywords = null,
    //    CancellationToken cancellationToken = default
    //)
    //{
    //    // 1) Girdi doğrulamaları
    //    if (phoneNumbers == null || phoneNumbers.Count == 0)
    //        throw new ArgumentException("Phone number list cannot be empty.", nameof(phoneNumbers));

    //    var fixedPhoneNumbers = phoneNumbers
    //   .Select(phoneNumber =>
    //   {
    //       // 2a) Telefon numarasındaki tüm boşlukları ve geçersiz karakterleri temizle
    //       var cleanedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

    //       // 2b) Türkiye numarasının 90 ile başlamadığını kontrol et
    //       if (!cleanedPhoneNumber.StartsWith("90"))
    //       {
    //           // Eğer numara "90" ile başlamıyorsa, düzeltme yap (örneğin, başına 90 ekleyebilirsin)
    //           cleanedPhoneNumber = "90" + cleanedPhoneNumber;
    //       }

    //       // 2c) Telefon numarasının 11 haneli olduğundan emin ol
    //       if (cleanedPhoneNumber.Length != 12)
    //       {
    //           // 11 haneli değilse, 11 haneli yapmak için kesebiliriz (veya hata fırlatabiliriz)
    //           cleanedPhoneNumber = cleanedPhoneNumber.Substring(0, 12); // veya alternatif düzeltme yapılabilir
    //       }

    //       return cleanedPhoneNumber;
    //   })
    //   .ToList();


    //    bool hasMessages = messages != null && messages.Count > 0;
    //    bool hasCustomKey = keywords != null;
    //    bool hasCustomMessage = !string.IsNullOrWhiteSpace(customMessage);

    //    if (!hasMessages && !hasCustomKey && !hasCustomMessage)
    //        throw new ArgumentException("At least one of messages, messageKey or customMessage must be provided.");

    //    // 2) Sağlayıcı ayarlarını al
    //    var providerKey = provider.ToString();
    //    if (!_config.SmsServices.TryGetValue(providerKey, out var apiInfo))
    //        throw new KeyNotFoundException($"SmsServices içinde '{providerKey}' bulunamadı.");

    //    var sender = _config.SmsConfiguration.Sender;

    //    // 3) finalMessages listesi
    //    List<string> finalMessages;
    //    if (hasCustomMessage)
    //    {
    //        finalMessages = Enumerable.Repeat(customMessage!, phoneNumbers.Count).ToList();
    //    }
    //    else if (hasCustomKey)
    //    {
    //        var keyName = messageKey!.Value.ToString();
    //        if (!_config.SmsConfiguration.Messages.TryGetValue(keyName, out var template))
    //            throw new KeyNotFoundException($"Message key '{keyName}' not found in configuration.");

    //        finalMessages = Enumerable
    //            .Repeat(template, phoneNumbers.Count)
    //            .Select(m =>
    //            {
    //                var result = m;
    //                var props = keywords.GetType().GetProperties();
    //                foreach (var prop in props)
    //                {
    //                    var value = prop.GetValue(keywords)?.ToString() ?? string.Empty;
    //                    result = result.Replace($"{{{prop.Name}}}", value);
    //                }
    //                return result;
    //            })
    //            .ToList();
    //    }
    //    else
    //    {
    //        // messages != null && Count>0
    //        if (messages!.Count == 1)
    //            finalMessages = Enumerable.Repeat(messages[0], phoneNumbers.Count).ToList();
    //        else if (messages.Count == phoneNumbers.Count)
    //            finalMessages = messages;
    //        else
    //            throw new ArgumentException("If providing messages list, its Count must be 1 or equal to phone number count.", nameof(messages));
    //    }

    //    // 4) Doğru API yolunu seç
    //    string path = phoneNumbers.Count switch
    //    {
    //        1 when finalMessages.Count == 1 => apiInfo.OTP,
    //        _ when finalMessages.Distinct().Count() == 1 => apiInfo.OTN,
    //        _ => apiInfo.NTN,
    //    };

    //    // 5) Payload oluştur
    //    var payload = new
    //    {
    //        api_id = apiInfo.ApiId,
    //        api_key = apiInfo.ApiKey,
    //        sender = sender,
    //        message_type = "normal",
    //        message = finalMessages.Count == 1 ? finalMessages[0] : null,
    //        messages = finalMessages.Count > 1 ? finalMessages : null,
    //        phones = phoneNumbers
    //    };

    //    var jsonOptions = new JsonSerializerOptions
    //    {
    //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    //    };
    //    var json = JsonSerializer.Serialize(payload, jsonOptions);
    //    var content = new StringContent(json, Encoding.UTF8, "application/json");

    //    var fullUrl = $"{apiInfo.ApiUrl.TrimEnd('/')}{path}";

    //    // 6) Çağrı ve sonuç işleme
    //    HttpResponseMessage response;
    //    try
    //    {
    //        response = await _httpClient.PostAsync(fullUrl, content, cancellationToken);
    //    }
    //    catch (Exception ex)
    //    {
    //        //_logger.LogError(ex, "SMS servisine istek atılırken hata oluştu.");
    //        throw ex;
    //    }

    //    var result = await response.Content.ReadAsStringAsync(cancellationToken);

    //    return result;
    //}

    public async Task<string> SendSmsAsync(
    SmsServices provider,
    List<string> phoneNumbers,
    List<string>? messages = null,
    SmsMessages? messageKey = SmsMessages.Custom,
    string? customMessage = null,
    SmsKeywordsDto? keywords = null,
    CancellationToken cancellationToken = default
)
    {
        // 1) Girdi doğrulamaları
        if (phoneNumbers == null || phoneNumbers.Count == 0)
            throw new ArgumentException("Phone number list cannot be empty.", nameof(phoneNumbers));

        var fixedPhoneNumbers = phoneNumbers
        .Select(phoneNumber =>
        {
            // 2a) Telefon numarasındaki tüm boşlukları ve geçersiz karakterleri temizle
            var cleanedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // 2b) Türkiye numarasının 90 ile başlamadığını kontrol et
            if (!cleanedPhoneNumber.StartsWith("90"))
            {
                // Eğer numara "90" ile başlamıyorsa, düzeltme yap (örneğin, başına 90 ekleyebilirsin)
                cleanedPhoneNumber = "90" + cleanedPhoneNumber;
            }

            // 2c) Telefon numarasının 11 haneli olduğundan emin ol
            if (cleanedPhoneNumber.Length != 12)
            {
                // 11 haneli değilse, 11 haneli yapmak için kesebiliriz (veya hata fırlatabiliriz)
                cleanedPhoneNumber = cleanedPhoneNumber.Substring(0, 12); // veya alternatif düzeltme yapılabilir
            }

            return cleanedPhoneNumber;
        })
        .ToList();

        bool hasMessages = messages != null && messages.Count > 0;
        bool hasCustomKey = keywords != null;
        bool hasCustomMessage = !string.IsNullOrWhiteSpace(customMessage);

        if (!hasMessages && !hasCustomKey && !hasCustomMessage)
            throw new ArgumentException("At least one of messages, messageKey or customMessage must be provided.");

        // 2) Sağlayıcı ayarlarını al
        var providerKey = provider.ToString();
        if (!_config.SmsServices.TryGetValue(providerKey, out var apiInfo))
            throw new KeyNotFoundException($"SmsServices içinde '{providerKey}' bulunamadı.");

        var sender = _config.SmsConfiguration.Sender;

        // AWS özel kontrolü
        if (provider == SmsServices.AMAZON)
        {
            // AWS SNS (Simple Notification Service) kullanarak SMS gönderimi
            var snsMessage = new PublishRequest
            {
                Message = customMessage ?? string.Join(" ", messages ?? new List<string>()),
                PhoneNumber = string.Join(",", fixedPhoneNumbers)
            };

            try
            {
                var amazonResponse = await _snsClient.PublishAsync(snsMessage, cancellationToken);
                return $"AWS SNS SMS gönderildi: {amazonResponse.MessageId}";
            }
            catch (Exception ex)
            {
                // AWS'ye özel hata işlemesi
                throw new Exception($"AWS SMS gönderimi sırasında hata oluştu: {ex.Message}", ex);
            }
        }

            List<string> finalMessages;
        if (hasCustomMessage)
        {
            finalMessages = Enumerable.Repeat(customMessage!, phoneNumbers.Count).ToList();
        }
        else if (hasCustomKey)
        {
            var keyName = messageKey!.Value.ToString();
            if (!_config.SmsConfiguration.Messages.TryGetValue(keyName, out var template))
                throw new KeyNotFoundException($"Message key '{keyName}' not found in configuration.");

            finalMessages = Enumerable
                .Repeat(template, phoneNumbers.Count)
                .Select(m =>
                {
                    var result = m;
                    var props = keywords.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(keywords)?.ToString() ?? string.Empty;
                        result = result.Replace($"{{{prop.Name}}}", value);
                    }
                    return result;
                })
                .ToList();
        }
        else
        {
            // messages != null && Count>0
            if (messages!.Count == 1)
                finalMessages = Enumerable.Repeat(messages[0], phoneNumbers.Count).ToList();
            else if (messages.Count == phoneNumbers.Count)
                finalMessages = messages;
            else
                throw new ArgumentException("If providing messages list, its Count must be 1 or equal to phone number count.", nameof(messages));
        }

        // 4) Doğru API yolunu seç
        string path = phoneNumbers.Count switch
        {
            1 when finalMessages.Count == 1 => apiInfo.OTP,
            _ when finalMessages.Distinct().Count() == 1 => apiInfo.OTN,
            _ => apiInfo.NTN,
        };

        // 5) Payload oluştur
        var payload = new
        {
            api_id = apiInfo.ApiId,
            api_key = apiInfo.ApiKey,
            sender = sender,
            message_type = "normal",
            message = finalMessages.Count == 1 ? finalMessages[0] : null,
            messages = finalMessages.Count > 1 ? finalMessages : null,
            phones = phoneNumbers
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var json = JsonSerializer.Serialize(payload, jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var fullUrl = $"{apiInfo.ApiUrl.TrimEnd('/')}{path}";

        // 6) Çağrı ve sonuç işleme
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsync(fullUrl, content, cancellationToken);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "SMS servisine istek atılırken hata oluştu.");
            throw ex;
        }

        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        return result;
    }


    public async Task<string> SendVerifyCodeAsync(SmsKeywordsDto keywords, string number, string operation)
    {
        var code = GenerateCode();
        var expirationDate = DateTime.UtcNow.AddMinutes(3);

        // 1. Kodu hashle
        var hashedCode = ComputeSha256Hash(code);

        // 2. JSON objesi hazırla
        var payload = new
        {
            hash = hashedCode,
            expirationDate = expirationDate,
            operation = operation
        };
        var json = JsonSerializer.Serialize(payload);

        // 3. Belleğe ata
        _verificationStore[number] = json;

        // 4. Sms'e yerleştir ve gönder
        keywords.code = code; // Şifrelenmemiş hali sms ile gidecek
        var result = await SendSmsAsync(
            SmsServices.AMAZON,
            new List<string> { number },
            messageKey: SmsMessages.Recourse,
            keywords: keywords
        );

        // 5. Sonucu kontrol et
        if (!string.IsNullOrWhiteSpace(result))
        {
            using var document = JsonDocument.Parse(result);
            var root = document.RootElement;

            int resultCode = root.GetProperty("code").GetInt32();
            string status = root.GetProperty("status").GetString();
            string description = root.GetProperty("description").GetString();

            if (status == "error" || resultCode != 200)
            {
                _verificationStore.TryRemove(number, out _);
            }
        }
        return result;
    }


    public bool VerifyCode(string number, string code, string operation)
    {
        if (!_verificationStore.TryGetValue(number, out var json))
            return false;

        var data = JsonSerializer.Deserialize<VerificationStorageDto>(json);

        if (data == null || data.expirationDate < DateTime.UtcNow)
            return false;

        if (data.operation != operation)
            return false;

        var hashedInput = ComputeSha256Hash(code);
        return data.hash == hashedInput;
    }

    public   bool HasPendingCode(string number, string operation)
    {
        if (!_verificationStore.TryGetValue(number, out var json))
            return false;

        var data = JsonSerializer.Deserialize<VerificationStorageDto>(json);

        return data is not null &&
               data.operation == operation &&
               data.expirationDate > DateTime.UtcNow;
    }


    // SHA256 Hash metodu
    private   string ComputeSha256Hash(string rawData)
    {
        using var sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        StringBuilder builder = new StringBuilder();
        foreach (var b in bytes)
            builder.Append(b.ToString("x2"));
        return builder.ToString();
    }

    private   string GenerateCode()
    {
        Random random = new Random();
        int number = random.Next(0, 1000000); 
        return number.ToString("D6");
    }

}

// Tek Kişiye Sabit Doğrulama kodu
//await _smsAdapter.SendSmsAsync(
//    SmsServices.VATAN,
//    new List<string> { "905xxxxxxxxx" },
//    messages: null,
//    messageKey: SmsMessages.Auth,
// keywords: new SmsKeywordsDto { code = "1234"}
//);


//birden fazla kişiye custom mesaj
//await _smsAdapter.SendSmsAsync(
//    SmsServices.VATAN,
//    new List<string> { "905xxxxxxxx1", "905xxxxxxxx2" },
//    messages: new List<string> { "Kampanyamıza hoş geldiniz!" }
//);


//herkese birer custom mesaj

//await _smsAdapter.SendSmsAsync(
//    SmsServices.VATAN,
//    new List<string> { "905xxxx1", "905xxxx2" },
//    messages: new List<string> { "Merhaba Ali", "Merhaba Ayşe" }
//);

//tek kişiye custom mesaj
//await _smsAdapter.SendSmsAsync(
//    SmsServices.VATAN,
//    new List<string> { "905xxxxxxxxx" },
//    messages: null,
//    customMessage: "Bu özel bir mesajdır."
//);

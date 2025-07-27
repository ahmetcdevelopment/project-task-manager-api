using NArchitecture.Core.Mailing.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NArchitecture.Core.Mailing.Helpers;
public static class HashHelper
{
    private static readonly ConcurrentDictionary<string, MailVerificationStorageDto> _memory =
        new ConcurrentDictionary<string, MailVerificationStorageDto>();

    // 1. Koddan hash üret, expirationDate ve operation ile kaydet
    public static void SaveCode(string code, DateTime expirationDate, string operation)
    {
        var hash = ComputeHash(code);
        var dto = new MailVerificationStorageDto
        {
            hash = hash,
            expirationDate = expirationDate,
            operation = operation
        };

        _memory[hash] = dto; // hash key olacak
    }


    public static bool CheckCodeSent(string operation)
    {
        // operation'a ait herhangi bir kayıt varsa ve süresi geçmemişse, kod zaten gönderilmiştir
        var existingCode = _memory.Values
                                   .FirstOrDefault(dto => dto.operation == operation);

        if (existingCode != null)
        {
            // Kod gönderilmiş ve süresi geçmişse, kaydı temizle
            if (existingCode.expirationDate <= DateTime.UtcNow)
            {
                _memory.TryRemove(existingCode.hash, out _);  // Süresi geçmişse hafızadan temizle
                return false;  // Kod geçersiz
            }

            return true; // Kod geçerli
        }

        return false; // Kod gönderilmemiş
    }

    public static void CleanCode(string operation)
    {
        var existingCode = _memory.Values
                                   .Where(dto => dto.operation == operation);

        foreach (var code in existingCode)
        {
            _memory.TryRemove(code.hash, out _);
        }
    }

    // 2. Kodun geçerli olup olmadığını kontrol et
    public static bool IsCodeValid(string code, string operation)
    {
        var hash = ComputeHash(code);

        if (_memory.TryGetValue(hash, out var dto))
        {
            // Operasyon aynı mı ve süre geçmemiş mi kontrolü
            return dto.operation == operation && dto.expirationDate > DateTime.UtcNow;
        }

        return false;
    }

    // Ortak hash metodu
    private static string ComputeHash(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        using (SHA256 sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }

    public static string GenerateCode()
    {
        Random random = new Random();
        int number = random.Next(0, 1000000);
        return number.ToString("D6");
    }
}
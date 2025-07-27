using System.Diagnostics;
using System.Security.Cryptography;

namespace NArchitecture.Core.Security.Encryption;
public static class AesCodeEncryptor
{
    // Kopyalama hatalarını engellemek için doğrudan byte dizisi olarak tanımlanmıştır.
    // Bu değerler güvenli bir şekilde saklanmalıdır.
    // Gerçek uygulamada bunları ortam değişkenleri veya bir sır yöneticisinden almalısın.

    // 32-byte (256-bit) AES Key
    private static readonly byte[] Key = new byte[]
    {
        0x50, 0x6E, 0x79, 0x48, 0x76, 0x61, 0x78, 0x4C, 0x49, 0x30, 0x31, 0x54, 0x4B, 0x5A, 0x6B, 0x78,
        0x55, 0x32, 0x6E, 0x64, 0x33, 0x72, 0x4F, 0x65, 0x37, 0x61, 0x42, 0x39, 0x63, 0x44, 0x30, 0x65
    };

    // 16-byte (128-bit) AES Initialization Vector (IV)
    private static readonly byte[] IV = new byte[]
    {
        0x7A, 0x66, 0x38, 0x6C, 0x33, 0x79, 0x31, 0x45, 0x35, 0x68, 0x37, 0x6B, 0x30, 0x6A, 0x34, 0x6E
    };

    // Statik kurucu (static constructor): Sınıf ilk kez kullanıldığında bir kez çalışır.
    static AesCodeEncryptor()
    {
        try
        {
            // Debug için anahtar ve IV uzunluklarını yazdıralım
            Debug.WriteLine($"[AesCodeEncryptor] Key Length: {Key.Length} bytes");
            Debug.WriteLine($"[AesCodeEncryptor] IV Length: {IV.Length} bytes");

            // AES-256 için anahtar 32 byte (256 bit), IV 16 byte (128 bit) olmalı
            if (Key.Length != 32)
            {
                throw new CryptographicException($"AES Key must be 32 bytes (256 bits). Current length: {Key.Length}. Key array is incorrectly defined.");
            }
            if (IV.Length != 16)
            {
                throw new CryptographicException($"AES IV must be 16 bytes (128 bits). Current length: {IV.Length}. IV array is incorrectly defined.");
            }
        }
        catch (Exception ex)
        {
            // Statik kurucudaki hataları yakala ve daha açıklayıcı bir mesajla fırlat
            Debug.WriteLine($"[AesCodeEncryptor ERROR] Unexpected error during static initialization: {ex.Message}");
            throw new TypeInitializationException(typeof(AesCodeEncryptor).FullName, ex);
        }
    }

    public static string Encrypt(string plainText)
    {
        // Anahtar ve IV'nin gerçekten yüklendiğinden emin olun (statik kurucu çalıştıysa null olmazlar)
        if (Key == null || IV == null)
        {
            throw new InvalidOperationException("Encryption keys (Key or IV) are not initialized. This indicates a severe issue during application startup or key loading.");
        }

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC; // Önerilen mod
            aesAlg.Padding = PaddingMode.PKCS7; // Önerilen doldurma modu

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes); // Şifreli byte dizisini Base64 olarak geri döndür
                }
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        if (Key == null || IV == null)
        {
            throw new InvalidOperationException("Encryption keys (Key or IV) are not initialized. This indicates a severe issue during application startup or key loading.");
        }

        // Gelen Base64 şifreli metni byte dizisine dönüştür
        byte[] cipherBytes;
        try
        {
            cipherBytes = Convert.FromBase64String(cipherText);
        }
        catch (FormatException ex)
        {
            Debug.WriteLine($"[AesCodeEncryptor ERROR] FormatException during Decrypt (Base64 to bytes): {ex.Message}");
            throw new ArgumentException("Provided cipherText is not a valid Base64 string.", nameof(cipherText), ex);
        }

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;
            aesAlg.Mode = CipherMode.CBC; // Şifreleme ile aynı mod olmalı
            aesAlg.Padding = PaddingMode.PKCS7; // Şifreleme ile aynı doldurma modu olmalı

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
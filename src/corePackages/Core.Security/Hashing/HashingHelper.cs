using System.Security.Cryptography;
using System.Text;

namespace Core.Security.Hashing;

public static class HashingHelper
{
    /// <summary>
    /// Create a password hash and salt via HMACSHA512.
    /// </summary>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// Verify a password hash and salt via HMACSHA512.
    /// </summary>
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new(passwordSalt);

        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    /// <summary>
    /// Converts a string to a byte array (BLOB) for database storage.
    /// </summary>
    public static byte[] StringToBlob(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    /// <summary>
    /// Converts a byte array (BLOB) back to a string.
    /// </summary>
    public static string BlobToString(byte[] blobData)
    {
        return Encoding.UTF8.GetString(blobData);
    }

    /// <summary>
    /// Generate a salt for a unique code.
    /// </summary>
    /// <param name="uniqueCode"></param>
    /// <returns></returns>
    public static byte[] GenerateSaltForCode(long uniqueCode)
    {
        // TC Kimlik No'yu sayılara ayırma (basamakları almak)
        var digits = new System.Collections.Generic.List<int>();
        while (uniqueCode > 0)
        {
            digits.Insert(0, (int)(uniqueCode % 10)); // Her bir basamağı alıyoruz
            uniqueCode /= 10;
        }

        // Matematiksel işlemle salt oluşturma
        long saltValue = 0;
        const int ModuloFactor = 997; // Modül işlemi için bir asal sayı (örnek)

        // Sayılardan matematiksel olarak salt türetme
        for (int i = 0; i < digits.Count; i++)
        {
            int factor = (i + 1) * 7; // Çarpan örneği
            saltValue += digits[i] * factor;  // Çarpanla işlemi yapıyoruz
        }

        // Modüler aritmetik ile dinamik bir sonuç elde edebiliriz
        saltValue = saltValue % ModuloFactor; // Modül işlemi ile sonuçları sınırlandırıyoruz

        // Salt'ı byte dizisine dönüştürme
        return BitConverter.GetBytes(saltValue);
    }


    /// <summary>
    /// Create a code hash using the provided salt via HMACSHA512.
    /// </summary>
    public static void CreateCodeHash(string password, byte[] passwordSalt, out byte[] passwordHash)
    {
        using HMACSHA512 hmac = new(passwordSalt);
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

}

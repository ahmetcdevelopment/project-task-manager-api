using System.Security.Cryptography;
using System.Text;

namespace Core.Security.Extensions;
public static class StrEnDec
{
    public static string Encrypt(string? plainText)
    {
        string key = "3aBWl/?macxsFgB'9VfJ_2c*Ha?Lf/U'";

        byte[] iv = new byte[16];
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string? cipherText)
    {
        try
        {
            string key = "3aBWl/?macxsFgB'9VfJ_2c*Ha?Lf/U'";

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText ?? "");

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return "EncDec Transaction Failed.";
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Extensions;
public class MD5Hash
{
    public static string MD5Hashed(string metin)
    {
        using (MD5 md5 = MD5.Create())
        {
            // Metni byte dizisine dönüştür
            byte[] veri = Encoding.UTF8.GetBytes(metin);

            // Byte dizisini MD5 ile şifrele
            byte[] hash = md5.ComputeHash(veri);

            // Şifrelenmiş metni hexadecimal formata dönüştür
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2")); // "x2" iki haneli hexadecimal format
            }

            // Şifrelenmiş metni döndür
            return sb.ToString();
        }
    }
}

using System.Security.Cryptography;
using System.Text;

namespace Test_Encryption;

public static class Encryptor2
{
    public static string EncryptData(string data, string password = "reyal")
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        var dataBytes = Encoding.UTF8.GetBytes(data);
        using var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
        using var aes = Aes.Create("AesManaged"); //new RijndaelManaged();
        aes.Padding = PaddingMode.ISO10126;

        using var encryptor = aes.CreateEncryptor(pdb.GetBytes(16), pdb.GetBytes(16));
        using var msEncrypt = new MemoryStream();
        using var encStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        encStream.Write(dataBytes, 0, dataBytes.Length);
        encStream.FlushFinalBlock();
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public static string DecryptData(string data, string password = "reyal")
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        var dataBytes = Convert.FromBase64String(data);

        using var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
        using var aes = Aes.Create("AesManaged"); //new RijndaelManaged();
        aes.Padding = PaddingMode.ISO10126;

        using var decryptor = aes.CreateDecryptor(pdb.GetBytes(16), pdb.GetBytes(16));
        using var msDecrypt = new MemoryStream(dataBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(csDecrypt);
        return reader.ReadToEnd();
    }
}

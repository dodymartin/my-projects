using System.Security.Cryptography;
using System.Text;

namespace Test_Encryption;

public static class Encryptor
{
    public static string EncryptData(string data, string password)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        return EncryptData(Encoding.UTF8.GetBytes(data), password, PaddingMode.ISO10126);
    }

    public static string DecryptData(string data, string password)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        return DecryptData(Convert.FromBase64String(data), password, PaddingMode.ISO10126);
    }

    private static string EncryptData(byte[] data, string password, PaddingMode paddingMode)
    {
        using var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
        using var aes = Aes.Create("AesManaged"); //new RijndaelManaged();
        aes.Padding = paddingMode;

        using var encryptor = aes.CreateEncryptor(pdb.GetBytes(32), pdb.GetBytes(16));
        using var msEncrypt = new MemoryStream();
        using var encStream = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(encStream);
        writer.Write(data);
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    private static string DecryptData(byte[] data, string password, PaddingMode paddingMode)
    {
        using var pdb = new PasswordDeriveBytes(password, Encoding.UTF8.GetBytes("Salt"));
        using var aes = Aes.Create("AesManaged"); //new RijndaelManaged();
        //aes.Padding = paddingMode;

        using var decryptor = aes.CreateDecryptor(pdb.GetBytes(32), pdb.GetBytes(16));
        using var msDecrypt = new MemoryStream(data);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(csDecrypt);
        return reader.ReadToEnd();
    }
}

using System.Collections.ObjectModel;

namespace MinimalApi.Api.Core;

public class Credential : ICredential
{
    public string Domain { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Collection<string> OldPasswords { get; set; }
    public bool IsDecrypted { get; set; }

    public void DecryptPassword(string key)
    {
        if (!IsDecrypted)
        {
            Password = Encryptor.DecryptData(Password, key);

            if (OldPasswords != null)
                for (var i = 0; i < OldPasswords.Count; i++)
                    OldPasswords[i] = Encryptor.DecryptData(OldPasswords[i], key);
            IsDecrypted = true;
        }
    }

    public string ToSqlPlusFormat()
    {
        if (Username.ToUpper() == "SYS")
            return Username + "/" + Password + " AS SYSDBA";
        return Username + "/" + Password;
    }
}

using System.Collections.ObjectModel;

namespace MinimalApi.Api.Core;

public interface ICredential
{
    string? Domain { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    Collection<string> OldPasswords { get; set; }
    bool IsDecrypted { get; set; }

    public void DecryptPassword(string key);
    public string ToSqlPlusFormat();
}

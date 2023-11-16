namespace MinimalApi.Api.Core;

public interface IDatabase
{
    string Name { get; set; }
    string? Sid { get; set; }
    string? InitialCatalog { get; set; }
    ConnectionType ConnectionType { get; set; }
    string? DataSource { get; set; }
    string UserName { get; set; }
    string EncryptedPassword { get; set; }
    string Password { get; }
    string ConnectString { get; }
    string DisplayName { get; }

    string GetConnectString(string databaseSid);
    string GetConnectString(string userName, string password);
    string GetConnectString(string databaseSid, string userName, string password);
}

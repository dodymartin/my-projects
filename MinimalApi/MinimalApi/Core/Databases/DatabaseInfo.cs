using System.Diagnostics.CodeAnalysis;

namespace MinimalApi.Api.Core;

public class DatabaseInfo : IDatabase
{
    public string Name { get; set; }
    public string Sid { get; set; }
    public string InitialCatalog { get; set; }
    public ConnectionType ConnectionType { get; set; }
    public string DataSource { get; set; }
    public string UserName { get; set; }
    public string EncryptedPassword { get; set; }
    public string Password => Encryptor.DecryptData(EncryptedPassword);
    public string ConnectString => !string.IsNullOrEmpty(DataSource) ? GetConnectString(DataSource) : GetConnectString(Sid);
    public string DisplayName => Name != Sid ? $"{Name} ({Sid})" : Name;

    public override string ToString() => DisplayName;

    public string GetConnectString(string dataSource) => GetConnectString(dataSource, UserName, Password);

    public string GetConnectString(string userName, string password) => GetConnectString(Sid, userName, password);

    public string GetConnectString(string dataSource, string userName, string password)
    {
        switch (ConnectionType)
        {
            case ConnectionType.Odbc:
                return $"DSN={dataSource};UID={userName};PWD={password};QueryTimeout=0";
            case ConnectionType.Oracle:
                if (!string.IsNullOrEmpty(dataSource))
                    return $"Data Source={dataSource};User Id={userName};Password={password};Connection Timeout=120;Load Balancing=True;";
                else
                    return $"User Id={UserName};Password={Password};Connection Timeout=120;";
            case ConnectionType.Rdb:
                return $"Driver={{Oracle Rdb Driver{(Environment.Is64BitProcess ? " 64 Bit" : string.Empty)}}};Svr={dataSource};Uid={userName};Pwd={password};Database=declare schema filename bpfa_db;Cls=ipfs_if;Dns='';";
            case ConnectionType.Sql:
                if (!string.IsNullOrEmpty(userName))
                    return $"Data Source={dataSource};Initial Catalog={InitialCatalog};Uid={userName};Pwd={password};pooling=false;encrypt=false;";
                else
                    return $"Data Source={dataSource};Initial Catalog={InitialCatalog};Integrated Security=SSPI;pooling=false;encrypt=false;";
            case ConnectionType.Jet:
                return $"Provider=Microsoft.jet.oledb.4.0;Data Source={dataSource};User ID={userName};Password={password};";
            default:
                return string.Empty;
        }
    }
}

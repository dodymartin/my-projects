namespace MinimalApi.Dal;

public enum Applications
{
    StratosConfigurationWebApiHost = 210
}

public enum DatabaseSchemaTypes
{
    Bpfs = 1,
    Other = 2,
    Pfs = 3,
    Rdb = 4,
    Tpfs = 5,
    Ipfs = 6,
    Retrotech = 7,
}

public enum DatabaseTypes
{
    Corporate = 1,
    Plant = 2,
    Pc = 3,
}

public enum EnvironmentTypes
{
    Production = 1,
    Development = 2,
    Test = 3,
    TcdQa = 4,
    TcdDevelopment = 5,
    TcdTest = 6,
    TcdQa2 = 7,
    TcdDev2 = 8,
    TcdQa3 = 9
}
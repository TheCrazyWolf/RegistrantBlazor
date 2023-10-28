namespace RegistrantApplication.Server.Configs;

public static class ConfigSrv
{
    public const long MaxFileSize = 10485760;

    public const long RecordsByPage = 10;
    public const long AuthTokenLifeTimInHour = 4;
    public const string ConnectionString = "Data Source=localDatabase.db";
}
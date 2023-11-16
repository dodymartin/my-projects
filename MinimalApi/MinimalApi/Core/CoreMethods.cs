using Newtonsoft.Json;

namespace MinimalApi.Api.Core;

public static class CoreMethods
{
    public static IConfigurationBuilder BuildConfiguration(string configPath)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        Console.WriteLine($"Environment: {environment}");
        return
            new ConfigurationBuilder()
                .SetBasePath(configPath)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment?.ToLower()}.json",
                    optional: true,
                    reloadOnChange: true)
                .AddJsonFile("nlog.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();
    }

    public static T? FindAndLoad<T>(string configPath, string fileName)
    {
        var filePath = FindFileUpTree(configPath, fileName);
        if (!File.Exists(filePath))
            throw new Exception($"The {fileName} file is not found looking up directory tree from current directory {configPath}.");
        return DeserializeFromJsonFile<T>(filePath);
    }

    public static T? FindAndLoad<T>(string configPath, string configSitePath, string fileName)
    {
        var filePath = Path.Combine(configPath, fileName);
        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(configSitePath, fileName);
            if (!File.Exists(filePath))
                throw new Exception($"The {fileName} file is not found in {configPath} or {configSitePath}.");
        }
        return DeserializeFromJsonFile<T>(filePath);
    }

    public static string FindFileUpTree(string directoryName, string fileName, params string[] subDirectoryNames)
    {
        // Check current directory for file to exist
        var fullFileName = Path.Combine(directoryName, fileName);
        if (File.Exists(fullFileName))
            return fullFileName;
        else
        {
            // Check for certain sub-directories, then for file
            foreach (var subDirectoryName in subDirectoryNames)
            {
                fullFileName = Path.Combine(directoryName, subDirectoryName, fileName);
                if (File.Exists(fullFileName))
                    return fullFileName;
            }

            // Get Parent directory and start searching there
            var parentDirectoryName = Directory.GetParent(directoryName)?.FullName;
            if (!string.IsNullOrEmpty(parentDirectoryName))
                return FindFileUpTree(parentDirectoryName, fileName, subDirectoryNames);
        }
        return string.Empty;
    }

    public static string GetMajorMinorVersion(string version)
    {
        var parts = version.Split('.');
        if (parts.Length > 1)
            return $"{parts[0]}.{parts[1]}";
        return version;
    }

    public static JsonSerializerSettings JsonSerializerSettings { get; } = new JsonSerializerSettings()
    {
        DefaultValueHandling = DefaultValueHandling.Ignore,
        Formatting = Newtonsoft.Json.Formatting.Indented
    };

    public static JsonSerializerSettings JsonSerializerSettingsTypes
    {
        get
        {
            var settings = new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto
            };

#if NET6_0_OR_GREATER
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            settings.SerializationBinder = new NetCoreSerializationBinder();
#endif

            return settings;
        }
    }

    #region Serialize/Deserialize

    #region To/From JsonFile

    public static T? DeserializeFromJsonFile<T>(string path, string name)
    {
        return DeserializeFromJsonFile<T>(Path.Combine(path, name));
    }

    public static T? DeserializeFromJsonFile<T>(string fullFileName, bool required = true)
    {
        if (!File.Exists(fullFileName))
            if (required)
                throw new FileNotFoundException(fullFileName);
            else
                return default;

        T? returnValue = default;
        using (var stream = new StreamReader(fullFileName))
        {
            returnValue = JsonConvert.DeserializeObject<T>(stream.ReadToEnd(), JsonSerializerSettings);
        }
        return returnValue;
    }

    public static void SerializeToJsonFile(object item, string path, string name)
    {
        SerializeToJsonFile(item, Path.Combine(path, name));
    }

    public static void SerializeToJsonFile(object item, string fullFileName)
    {
        var fi = new FileInfo(fullFileName);
        if (fi is not null && !Directory.Exists(fi.DirectoryName))
            Directory.CreateDirectory(fi.DirectoryName!);

        var json = JsonConvert.SerializeObject(item, JsonSerializerSettings);
        using var stream = new StreamWriter(fullFileName);
        stream.Write(json);
    }

    #endregion

    #endregion
}

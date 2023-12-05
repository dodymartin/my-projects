using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace MinimalApi.Api.Core;

//Required by .Net Framework apps to deserialize .Net Core Json using passing data types
//Can be removed once we are completely on .Net Core
public partial class NetCoreSerializationBinder : DefaultSerializationBinder
{

    [GeneratedRegex(@"System\.Private\.CoreLib(, Version=[\d\.]+)?(, Culture=[\w-]+)(, PublicKeyToken=[\w\d]+)?")]
    private static partial Regex MyRegex();

    private static readonly ConcurrentDictionary<Type, (string assembly, string type)> _cache = new();

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        base.BindToName(serializedType, out assemblyName!, out typeName!);

        if (_cache.TryGetValue(serializedType, out var name))
        {
            assemblyName = name.assembly;
            typeName = name.type;
        }
        else
        {
            if (!assemblyName!.StartsWith("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase))
                assemblyName = MyRegex().Replace(assemblyName, "mscorlib");

            if (!typeName!.StartsWith("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase))
                typeName = MyRegex().Replace(typeName, "mscorlib");

            _cache.TryAdd(serializedType, (assemblyName, typeName));
        }
    }
}

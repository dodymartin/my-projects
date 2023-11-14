using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace MinimalApi.Api.Core;

//Required by .Net Framework apps to deserialize .Net Core Json using passing data types
//Can be removed once we are completely on .Net Core
public class NetCoreSerializationBinder : DefaultSerializationBinder
{
    private static readonly Regex _regex = new(
        @"System\.Private\.CoreLib(, Version=[\d\.]+)?(, Culture=[\w-]+)(, PublicKeyToken=[\w\d]+)?");

    private static readonly ConcurrentDictionary<Type, (string assembly, string type)> _cache = new();

    public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        base.BindToName(serializedType, out assemblyName, out typeName);

        if (_cache.TryGetValue(serializedType, out var name))
        {
            assemblyName = name.assembly;
            typeName = name.type;
        }
        else
        {
            if (assemblyName.IndexOf("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase) != 0)
                assemblyName = _regex.Replace(assemblyName, "mscorlib");

            if (typeName.IndexOf("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase) != 0)
                typeName = _regex.Replace(typeName, "mscorlib");

            _cache.TryAdd(serializedType, (assemblyName, typeName));
        }
    }
}

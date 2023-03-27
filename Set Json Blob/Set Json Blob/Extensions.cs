using System;
using System.Collections.Generic;
using System.Linq;
using Cap.Ipfs.Base;

namespace Set_Json_Blob
{
    internal static class Extensions
    {
        internal static Type[] GetTypes(this IEnumerable<ReportFilters> reportFilterSets)
        {
            var types = new List<Type>();
            foreach (var reportFilterSet in reportFilterSets)
                types = types.Union(reportFilterSet.GetTypes().ToList()).ToList();
            return types.ToArray();
        }
    }
}

using System.Collections.ObjectModel;

namespace Load_Install_Cache
{
    internal class Applications : KeyedCollection<string, KeyValuePair<string, int>>
    {
        protected override string GetKeyForItem(KeyValuePair<string, int> item)
        {
            return item.Key;
        }
    }
}

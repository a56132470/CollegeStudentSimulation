using System.Collections.Generic;

namespace Framework.UI.Tools
{
    public static class DictTool
    {
        public static Tvalue GetValue<TKey, Tvalue>(this Dictionary<TKey, Tvalue> dict, TKey key)
        {
            dict.TryGetValue(key, out var value);
            return value;
        }
    }
}
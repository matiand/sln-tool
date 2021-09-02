using System.Collections.Specialized;

namespace SolutionFile.Extensions
{
    public static class OrderedDictionaryExtensions
    {
        public static bool TryAdd(this OrderedDictionary dict, string key, string value)
        {
            if (dict.Contains(key)) return false;

            dict.Add(key, value);
            return true;
        }
    }
}

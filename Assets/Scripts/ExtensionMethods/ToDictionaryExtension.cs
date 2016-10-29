using System.Collections.Generic;
using System;

namespace SpaceCentipedeFromHell
{
    public static class ToDictionaryExtension
    {
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var result = new Dictionary<TKey, TSource>();

            foreach (var item in source) result.Add(keySelector(item), item);

            return result;
        }

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<KeyValuePair<TKey, TSource>> source)
        {
            var result = new Dictionary<TKey, TSource>();

            foreach (var item in source) result.Add(item.Key, item.Value);

            return result;
        }
    }
}
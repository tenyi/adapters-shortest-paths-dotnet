using System.Collections.Generic;

namespace JavaToDotNetTranslationHelpers {

    // IEnumerable<T> (and also ICollection<T>) are base types for e.g. Dictionary,List,HashSet.

    // Dictionary/IDictionary:
    // public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>,  IReadOnlyCollection<KeyValuePair<TKey, TValue>>
    // public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>
    // https://msdn.microsoft.com/en-us/library/xfhwa508(v=vs.110).aspx
    // https://msdn.microsoft.com/en-us/library/s4ys34ea(v=vs.110).aspx

    // HashSet/ISet:
    // public class HashSet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IReadOnlyCollection<T>
    // public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    // https://msdn.microsoft.com/en-us/library/bb359438(v=vs.110).aspx
    // https://msdn.microsoft.com/en-us/library/dd412081(v=vs.110).aspx

    // public class ReadOnlyCollection<T> : IList<T>, ICollection<T>,  IEnumerable<T>, IReadOnlyList<T>, IReadOnlyCollection<T>
    // https://msdn.microsoft.com/en-us/library/ms132474(v=vs.110).aspx

    // public interface IReadOnlyCollection<out T> : IEnumerable<T>
    // https://msdn.microsoft.com/en-us/library/hh881542(v=vs.110).aspx

    // Note that extension methods also works for .NET 2
    public static class ExtensionMethodsForCollectionsAndDictionaries {
        public static void AddAll<T>(
            this ICollection<T> list, 
            // e.g. ICollection<KeyValuePair<TKey, TValue>> for a Dictionary
            IEnumerable<T> list2
        ) {
            // This method can also be used for Dictionary
            // instead of using such a method as below:
            //  parameters: this IDictionary<T,U> dict1, IDictionary<T,U> dict2
            //  foreach(var x in dict2) {
            //    dict1.Add(x.Key, x.Value);
            //  }
            foreach(T t in list2) {
                list.Add(t);
            }
        }

        public static void AddAll<T>(
            this ICollection<T> list, 
            List<T> list2
        ) {
            foreach(T t in list2) {
                list.Add(t);
            }
        }

        // 簡化為原生字典索引器操作，避免先 ContainsKey 再 Remove 的額外開銷。
        public static void AddOrReplace<T,U>(
            this IDictionary<T,U> dict, 
            T t, 
            U u
        ) {
            dict[t] = u;
        }

        // 使用 TryGetValue 取代 ContainsKey + 索引器取值，減少一次雜湊定位以提升效能。
        public static U GetValueIfExists<T,U>(
            this IDictionary<T, U> dict, 
            T key
        ) {
            if (dict.TryGetValue(key, out var val)) {
                return val;
            }
            return default(U);
        }
    }
}
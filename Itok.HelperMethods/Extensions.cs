using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Itok.HelperMethods
{
    public static class Extensions
    {
        public static IEnumerable<TReturn>
            DeserializeFromStream<TReturn>(this Stream stream) //streaming jObjectFrom JsonArray File.
        {
            using (var sr = new StreamReader(stream))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            yield return JObject.Load(reader).ToObject<TReturn>();
                        }
                    }
                }
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static IEnumerable<List<T>> SplitListByCount<T>(this List<T> list, int nSize)
        {
            for (var i = 0; i < list.Count; i += nSize) yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
        }

#pragma warning disable S4456 // Parameter validation in yielding methods should be wrapped

        public static IEnumerable<int> AllIndexesOf(this string str, string value,
#pragma warning restore S4456 // Parameter validation in yielding methods should be wrapped
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            }

            for (var index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, comparison);
                if (index == -1)
                {
                    break;
                }

                yield return index;
            }
        }

        //https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 352654597;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }
}
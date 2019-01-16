using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Itok.HelperMethods
{
    public static class Extensions
    {
        /// <summary>
        ///     DeserializeJsonArrayFromStream
        /// </summary>
        /// <typeparam name="TReturn">ReturnType</typeparam>
        /// <param name="stream">InputStream</param>
        /// <returns></returns>
        public static IEnumerable<TReturn>
            DeserializeFromStream<TReturn>(this Stream stream) //streaming jObjectFrom JsonArray File.
        {
            using (var sr = new StreamReader(stream))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    while (reader.Read())
                        if (reader.TokenType == JsonToken.StartObject)
                            yield return JObject.Load(reader).ToObject<TReturn>();
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

        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty(str))
                throw new ArgumentException("the string to find may not be empty", nameof(value));

            for (var index = 0;; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.CurrentCultureIgnoreCase);
                if (index == -1) break;

                yield return index;
            }
        }
    }
}
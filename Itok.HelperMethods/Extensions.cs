using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Itok.HelperMethods
{
    public static class Extensions
    {
        /// <summary>
        /// DeserializeJsonArrayFromStream
        /// </summary>
        /// <typeparam name="TReturn">ReturnType</typeparam>
        /// <param name="stream">InputStream</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> DeserializeFromStream<TReturn>(this Stream stream)//streaming jObjectFrom JsonArray File.
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
            for (var i = 0; i < list.Count; i += nSize)
            {
                yield return list.GetRange(i, Math.Min(nSize, list.Count - i));
            }
        }
    }
}

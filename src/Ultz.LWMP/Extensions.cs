using System;
using System.Collections.Generic;
using System.Linq;

namespace Ultz.LWMP
{
    public static class Extensions
    {
        public static T MoveOne<T>(this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable?.GetEnumerator();
            if (enumerator?.MoveNext() ?? false)
            {
                enumerator.Dispose();
                return enumerator.Current;
            }

            enumerator?.Dispose();
            return default;
        }
        
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            var left = array.Length;
            while (left != 0)
            {
                if (left > size)
                {
                    yield return new ArraySegment<T>(array, array.Length - left, size);
                    left -= size;
                }
                else if (left == size)
                {
                    yield return new ArraySegment<T>(array, array.Length - left, size);
                    yield return new T[0]; // return an empty array to indicate the end of the chunk
                    left -= size;
                }
                else
                {
                    yield return new ArraySegment<T>(array, array.Length - left, left);
                    left = 0;
                }
            }

            if (array.Length == 0)
            {
                yield return new T[0];
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    /// <summary>
    /// Better Lambda
    /// </summary>
    public static class EnumerableHelper
    {             
        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static string ForEachToString<T>(this List<T> source, string seperator = null)
        {
            var s = string.Empty;
            for (int i = 0; i < source.Count; i++)
            {
                s += source[i].ToString();
                if (seperator != null && i + 1 < source.Count)
                {
                    s += seperator;
                }
            }

            return s;
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static string ForEachToString<T>(this T[] source, string seperator = null)
        {
            var s = string.Empty;
            for (int i = 0; i < source.Length; i++)
            {
                s += source[i].ToString();
                if (seperator != null && i + 1 < source.Length)
                {
                    s += seperator;
                }
            }

            return s;
        }
        
        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static int UniquedAdd<T>(this List<T> source, T item)
        {
            var comparer = Comparer<T>.Default;

            if (source.Count == 0)
            {
                source.Insert(0, item);
                return 0;
            }

            for (int i = 0; i < source.Count; i++)
            {
                if(EqualityComparer<T>.Default.Equals(item, source[i]))
                {
                    source[i] = item;
                    return i;
                }

                var p = comparer.Compare(item, source[i]);

                if (p < 0)
                {
                    source.Insert(i, item);
                    return i;
                }
            }

            source.Insert(source.Count, item);
            return source.Count - 1;
        }


        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static int SortedAdd<T>(this List<T> source, T item)
        {
            var comparer = Comparer<T>.Default;

            if (source.Count == 0 || comparer == null)
            {
                source.Insert(0, item);
                return 0;
            }

            for (int i = 0; i < source.Count; i++)
            {
                var p = comparer.Compare(item, source[i]);

                if (p < 0)
                {
                    source.Insert(i, item);
                    return i;
                }
                if (p == 0)
                {
                    source.Insert(i, item);
                    return i;
                }
            }

            source.Insert(source.Count, item);
            return source.Count - 1;
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Next<T>(this List<T> list, T current)
        {
            if (list == null || list.Count == 0)
                return current;

            if (current == null)
                return list[0];

            var index = list.IndexOf(current);

            index++;

            if (index >= list.Count)
            {
                index = 0;
            }

            return list[index];
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Next<T>(this T[] list, T current)
        {
            if (list == null || list.Length == 0)
                return current;

            if (current == null)
                return list[0];

            var index = Array.IndexOf(list, current);

            index++;

            if (index >= list.Length)
            {
                index = 0;
            }

            return list[index];
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Back<T>(this List<T> list, T current)
        {
            if (list == null || list.Count == 0)
                return current;

            if (current == null)
                return list[0];

            var index = list.IndexOf(current);

            index--;

            if (index < 0)
            {
                index = list.Count - 1;
            }

            return list[index];
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Back<T>(this T[] list, T current)
        {
            if (list == null || list.Length == 0)
                return current;

            if (current == null)
                return list[0];

            var index = Array.IndexOf(list, current);

            index--;

            if (index < 0)
            {
                index = list.Length - 1;
            }

            return list[index];
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Random<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static T Random<T>(this T[] list)
        {
            if (list.Length == 0)
                return default(T);

            return list[UnityEngine.Random.Range(0, list.Length)];
        }

        public static T WeigtedRandom<T>(this List<T> list, Func<T, int> weight)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            int totalweight = list.Sum(c => weight(c));
            int choice = UnityEngine.Random.Range(0, totalweight);
            int sum = 0;

            foreach (var obj in list)
            {
                for (int i = sum; i < weight(obj) + sum; i++)
                {
                    if (i >= choice)
                    {
                        return obj;
                    }
                }
                sum += weight(obj);
            }

            return list.First();
        }

        /// <summary>
        /// EnumerableExt
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var k = UnityEngine.Random.Range(0, i + 1);
                var value = list[k];
                list[k] = list[i];
                list[i] = value;
            }

            return list;
        }
    }
}
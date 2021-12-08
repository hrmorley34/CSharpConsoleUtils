using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUtils.ConsoleImagery.Util
{
}

namespace ConsoleUtils.ConsoleImagery.Util.Linq
{
    /// <summary>An object and its index in an IEnumerable&lt;T&gt;</summary>
    public class EnumeratedObject<T> : System.Runtime.CompilerServices.ITuple
    {
        /// <summary>Index of the object</summary>
        public readonly int Index;
        /// <summary>The object</summary>
        public readonly T Object;

        /// <summary>Create a new EnumeratedObject</summary>
        public EnumeratedObject(int index, T obj)
        {
            Index = index;
            Object = obj;
        }

        /// <summary>Length of tuple</summary>
        public int Length { get => 2; }
        /// <summary>Tuple items</summary>
        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Index;
                    case 1: return Object;
                }
                return null;
            }
        }

        /// <summary>Deconstruct into index and object</summary>
        public void Deconstruct(out int index, out T obj)
        {
            index = Index;
            obj = Object;
        }
    }

    public static class EnumerableExtensions
    {
        /// <summary>Return each item of an enumerable along with its index</summary>
        public static IEnumerable<EnumeratedObject<T>> Enumerate<T>(this IEnumerable<T> collection)
            => collection.Select((item, index) => new EnumeratedObject<T>(index, item));

        /// <summary>Turn a jagged array into a 2D array</summary>
        public static T[,] Dejagged<T>(this T[][] arr) => Dejagged<T, T>(arr, t => t);

        /// <summary>Turn a jagged array into a 2D array, mapping each element using <c>map</c></summary>
        public static MapT[,] Dejagged<InT, MapT>(this InT[][] arr, Func<InT, MapT> map)
        {
            int height = arr.Length;
            if (height <= 0) return new MapT[0, 0];

            int width = arr[0].Length;

            MapT[,] newarr = new MapT[width, height];
            for (int y = 0; y < height; y++)
            {
                if (arr[y].Length != width) throw new ArgumentException("Array not square", nameof(arr));
                for (int x = 0; x < width; x++)
                {
                    newarr[x, y] = map(arr[y][x]);
                }
            }
            return newarr;
        }

        /// <summary>Turn a jagged array into a 2D array</summary>
        public static T[,] Dejagged<T>(this IEnumerable<IEnumerable<T>> arr)
            => Dejagged<T, T>(arr, t => t);

        /// <summary>Turn a jagged array into a 2D array, mapping each element using <c>map</c></summary>
        public static MapT[,] Dejagged<InT, MapT>(this IEnumerable<IEnumerable<InT>> arr, Func<InT, MapT> map)
            => Dejagged<InT, MapT>(arr.Select(e => e.ToArray()).ToArray(), map);
    }
}
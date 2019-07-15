using System.Collections.Generic;
using System.Linq;

namespace Alturos.ImageAnnotation.Helper
{
    public static class EnumerableExtensions
    {
        public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var jaggedArray = source
                .Select(x => x.ToArray())
                .ToArray();

            var array = new T[jaggedArray.Length, jaggedArray.Max(x => x.Length)];
            for (var i = 0; i < jaggedArray.Length; i++)
            {
                for (var j = 0; j < jaggedArray[i].Length; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }

            return array;
        }
    }
}

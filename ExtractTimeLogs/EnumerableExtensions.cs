using System;
using System.Collections.Generic;

namespace ExtractTimeLogs
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TItem> Run<TItem, TRunner>(this IEnumerable<TItem> source, Func<TRunner, TItem, (TRunner, TItem)> func, TRunner seed)
        {
            var currentRunner = seed;

            foreach (var item in source)
            {
                var (nextRunner, outputItem) = func(currentRunner, item);

                yield return outputItem;

                currentRunner = nextRunner;
            }
        }
    }
}

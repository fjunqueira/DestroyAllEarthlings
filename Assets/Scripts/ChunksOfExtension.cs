using System;
using System.Collections.Generic;
using System.Linq;

public static class ChunksOfExtension
{
    public static IEnumerable<IEnumerable<TResult>> ChunksOf<TResult>(this IEnumerable<TResult> enumerable, int size)
    {
        if (enumerable == null) throw new ArgumentNullException("enumerable");

        var chunk = new List<TResult>();

        for (int i = 0; i < enumerable.Count(); i++)
        {
            chunk.Add(enumerable.ElementAt(i));

            if ((i + 1) % size == 0)
            {
                yield return chunk;
                chunk = new List<TResult>();
            }
        }
    }
}
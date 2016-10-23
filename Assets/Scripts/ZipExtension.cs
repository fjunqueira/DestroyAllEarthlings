using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCentipedeFromHell
{
    public static class ZipExtension
    {
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            using (var iteratorA = first.GetEnumerator())
            using (var iteratorB = second.GetEnumerator())
            {
                while (iteratorA.MoveNext() && iteratorB.MoveNext())
                    yield return resultSelector(iteratorA.Current, iteratorB.Current);
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace DestroyAllEarthlings
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

        public static IEnumerable<TResult> Zip3<TFirst, TSecond, TThird, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (third == null) throw new ArgumentNullException("third");

            using (var iteratorA = first.GetEnumerator())
            using (var iteratorB = second.GetEnumerator())
            using (var iteratorC = third.GetEnumerator())
            {
                while (iteratorA.MoveNext() && iteratorB.MoveNext() && iteratorC.MoveNext())
                    yield return resultSelector(iteratorA.Current, iteratorB.Current, iteratorC.Current);
            }
        }
    }
}
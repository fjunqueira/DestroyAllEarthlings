using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DestroyAllEarthlings
{

    public static class ShiftExtension
    {
        public static TList Shift<TList>(this IList<TList> list)
        {
            if (list.Count() == 0) return default(TList);

            var result = list.First();
            list.RemoveAt(0);
            return result;
        }
    }
}
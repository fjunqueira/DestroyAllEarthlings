using System.Collections.Generic;

namespace SpaceCentipedeFromHell.Tests
{
    public class HeapIntComparer : IComparer<HeapInt>
    {
        public int Compare(HeapInt x, HeapInt y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null || y == null)
            {
                return 2;
            }

            if (x < y)
            {
                return -1;
            }

            if (x > y)
            {
                return 1;
            }

            return -2;
        }
    }
}
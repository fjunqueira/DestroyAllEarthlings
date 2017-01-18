using System.Collections.Generic;

namespace DestroyAllEarthlings.Tests
{
    public class TestObjectComparator : IComparer<TestObject>
    {
        public int Compare(TestObject x, TestObject y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null || y == null)
            {
                return 2;
            }

            if (x.Value < y.Value)
            {
                return -1;
            }

            if (x.Value > y.Value)
            {
                return 1;
            }

            return -2;
        }
    }
}
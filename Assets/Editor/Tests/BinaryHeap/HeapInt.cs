namespace DestroyAllEarthlings.Tests
{
    public class HeapInt : IHeapItem
    {
        private int underlyingValue;

        public int HeapIndex { get; set; }

        public static implicit operator HeapInt(int value)
        {
            return new HeapInt { underlyingValue = value };
        }

        public static implicit operator int(HeapInt value)
        {
            return value.underlyingValue;
        }
    }
}
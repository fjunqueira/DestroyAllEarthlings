﻿using System;

namespace SpaceCentipedeFromHell.Tests
{
    public class TestObject : IHeapItem
    {
        public TestObject(int value)
        {
            this.Value = value;
        }

        public int Value { get; set; }

        public int HeapIndex { get; set; }
    }
}
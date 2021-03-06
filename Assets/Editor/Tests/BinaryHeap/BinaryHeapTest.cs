﻿using NUnit.Framework;
using System;

namespace DestroyAllEarthlings.Tests
{
public class BinaryHeapTests
    {
        [Test]
        public void AssertThatSmallestValueIsOnTop()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            heap.Push((HeapInt)90);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)1243);
            heap.Push((HeapInt)10);
            heap.Push((HeapInt)56);
            heap.Push((HeapInt)22);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)754);
            heap.Push((HeapInt)77765);
            heap.Push((HeapInt)45675);

            Assert.AreEqual(10, heap.Pop());
        }

        [Test]
        public void AssertRemovingComplexObjectAtPositionDontBreakHeap()
        {
            var heap = new BinaryHeap<TestObject>(15, new TestObjectComparator());

            var item1 = new TestObject(384);
            var item2 = new TestObject(390);
            var item3 = new TestObject(404);
            var item4 = new TestObject(404);

            heap.Push(item3);
            heap.Push(item2);
            heap.Push(item4);
            heap.Push(item1);
            heap.Push(new TestObject(410));
            heap.Push(new TestObject(424));
            heap.Push(new TestObject(424));
            heap.Push(new TestObject(424));

            heap.Remove(item1);
            Assert.AreEqual(item2, heap.Pop());

            heap.Remove(item3);
            Assert.AreEqual(item4, heap.Pop());
        }

        [Test]
        public void AssertRemovingMultipleComplexObjectAtPositionDontBreakHeap()
        {
            var heap = new BinaryHeap<TestObject>(15, new TestObjectComparator());

            var item1 = new TestObject(410);
            var item2 = new TestObject(424);
            var item3 = new TestObject(424);
            var item4 = new TestObject(404);
            var item5 = new TestObject(390);
            var item6 = new TestObject(404);
            var item7 = new TestObject(384);
            var item8 = new TestObject(404);
            var item9 = new TestObject(424);

            heap.Push(item4);
            heap.Push(item5);
            heap.Push(item8);
            heap.Push(item7);
            heap.Push(item6);
            heap.Push(item1);
            heap.Push(item2);
            heap.Push(item3);
            heap.Push(item9);

            heap.Remove(item1);
            heap.Remove(item2);
            heap.Remove(item3);
            heap.Remove(item4);
            heap.Remove(item5);
            heap.Remove(item6);
            heap.Remove(item7);

            Assert.AreEqual(item8, heap.Pop());
            Assert.AreEqual(item9, heap.Pop());

            Assert.IsTrue(heap.IsEmpty);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void AssertRemovingFromEmptyHeapThrowsException()
        {
            var heap = new BinaryHeap<TestObject>(15, new TestObjectComparator());

            heap.Remove(new TestObject(404));
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void AssertThatFullExceptionIsThrown()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            heap.Push((HeapInt)90);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)1243);
            heap.Push((HeapInt)10);
            heap.Push((HeapInt)56);
            heap.Push((HeapInt)22);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)58);
            heap.Push((HeapInt)97);
            heap.Push((HeapInt)753);
            heap.Push((HeapInt)54675);
        }

        [Test]
        public void AssertThatHeapIsEmpty()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            Assert.IsTrue(heap.IsEmpty);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void AssertThatPoppingEmptyHeapThrowsException()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            heap.Pop();
        }

        [Test]
        public void AssertThatPoppingHeapUntilEmptyWorks()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            heap.Push((HeapInt)10);
            heap.Push((HeapInt)17);
            heap.Push((HeapInt)20);
            heap.Push((HeapInt)30);
            heap.Push((HeapInt)38);
            heap.Push((HeapInt)30);
            heap.Push((HeapInt)24);
            heap.Push((HeapInt)34);

            var firstItem = heap.Pop();

            while (firstItem != 38)
            {
                firstItem = heap.Pop();
            }

            Assert.IsTrue(heap.IsEmpty);
        }

        [Test]
        public void AssertThatPopAlwaysReturnsSmallestValue()
        {
            var heap = new BinaryHeap<HeapInt>(10, new HeapIntComparer());

            heap.Push((HeapInt)90);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)1243);
            heap.Push((HeapInt)10);
            heap.Push((HeapInt)56);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)75);
            heap.Push((HeapInt)97);
            heap.Push((HeapInt)753);
            heap.Push((HeapInt)54675);

            var result = new int?[10];
            var index = 0;

            var firstItem = heap.Pop();

            while (!heap.IsEmpty)
            {
                result[index] = firstItem;

                firstItem = heap.Pop();
                index++;
            }

            result[index] = firstItem;

            CollectionAssert.AreEqual(new int?[] { 10, 56, 75, 75, 75, 90, 97, 753, 1243, 54675 }, result);
        }

        [Test]
        public void AssertHeapWorksWithClasses()
        {
            var heap = new BinaryHeap<TestObject>(15, new TestObjectComparator());

            heap.Push(new TestObject(404));
            heap.Push(new TestObject(390));
            heap.Push(new TestObject(404));
            heap.Push(new TestObject(384));
            heap.Push(new TestObject(404));
            heap.Push(new TestObject(410));
            heap.Push(new TestObject(424));
            heap.Push(new TestObject(424));
            heap.Push(new TestObject(424));

            var result = new int[9];
            var index = 0;

            var firstItem = heap.Pop();

            while (!heap.IsEmpty)
            {
                result[index] = firstItem.Value;

                firstItem = heap.Pop();
                index++;
            }

            result[index] = firstItem.Value;

            CollectionAssert.AreEqual(new int[] { 384, 390, 404, 404, 404, 410, 424, 424, 424 }, result);
        }
    }
}
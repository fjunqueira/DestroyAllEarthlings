using System.Collections.Generic;
using System;
using UnityEngine;

namespace DestroyAllEarthlings
{
    public class BinaryHeap<T> where T : IHeapItem
    {
        private T[] heap;

        private int size;

        private int lastPositionAvaliable = 1;

        private IComparer<T> comparer;

        public BinaryHeap(int size, IComparer<T> comparer)
        {
            this.size = size;
            this.heap = new T[this.size + 1];
            this.comparer = comparer;
        }

        public bool IsEmpty
        {
            get
            {
                return this.lastPositionAvaliable == 1;
            }
        }

        private int LastPosition
        {
            get
            {
                return this.lastPositionAvaliable - 1;
            }
        }

        public void Push(T item)
        {
            if (this.lastPositionAvaliable > this.size)
            {
                throw new Exception("Binary Heap is full.");
            }

            this.PlaceAt(this.lastPositionAvaliable, item);

            this.Push(this.lastPositionAvaliable);

            this.lastPositionAvaliable++;
        }

        public T Pop()
        {
            if (this.IsEmpty)
            {
                throw new Exception("Binary Heap is empty.");
            }

            var item = this.heap[1];

            this.PlaceAt(1, this.heap[this.LastPosition]);

            this.PlaceAt(this.LastPosition, default(T));

            this.lastPositionAvaliable--;

            this.Pop(1);

            return item;
        }

        public T Remove(T obj)
        {
            if (this.IsEmpty)
            {
                throw new Exception("Binary Heap is empty.");
            }

            var item = this.heap[obj.HeapIndex];

            this.PlaceAt(obj.HeapIndex, this.heap[this.LastPosition]);

            this.PlaceAt(this.LastPosition, default(T));

            this.lastPositionAvaliable--;

            this.Pop(obj.HeapIndex);

            return item;
        }

        public T Peek()
        {
            if (this.IsEmpty)
            {
                return default(T);
            }

            return this.heap[1];
        }

        private void Pop(int index)
        {
            var leftChildIndex = this.GetLeftChildPosition(index);

            var rightChildIndex = this.GetRightChildPosition(index);

            var item = this.heap[index];

            var leftChild = default(T);
            var rightChild = default(T);

            if (leftChildIndex <= this.size)
            {
                leftChild = this.heap[leftChildIndex];
            }

            if (rightChildIndex <= this.size)
            {
                rightChild = this.heap[rightChildIndex];
            }

            if ((this.comparer.Compare(item, leftChild) < 0 || object.Equals(leftChild, default(T))) && (this.comparer.Compare(item, rightChild) < 0 || object.Equals(rightChild, default(T))))
            {
                return;
            }

            if ((this.comparer.Compare(leftChild, rightChild) <= 0) || object.Equals(rightChild, default(T)))
            {
                this.PlaceAt(index, leftChild);
                this.PlaceAt(leftChildIndex, item);

                this.Pop(leftChildIndex);
            }
            else if (this.comparer.Compare(rightChild, leftChild) < 0)
            {
                this.PlaceAt(index, rightChild);
                this.PlaceAt(rightChildIndex, item);

                this.Pop(rightChildIndex);
            }
        }

        private void Push(int index)
        {
            var item = this.heap[index];

            var parentIndex = this.GetParentPositionInHeap(index);

            var parent = this.heap[parentIndex];

            if (this.comparer.Compare(item, parent) < 0)
            {
                this.PlaceAt(index, parent);
                this.PlaceAt(parentIndex, item);

                this.Push(parentIndex);
            }
        }

        private void PlaceAt(int index, T value)
        {
            if (!object.Equals(value, default(T))) value.HeapIndex = index;

            this.heap[index] = value;
        }

        private int GetRightChildPosition(int index)
        {
            return (index * 2) + 1;
        }

        private int GetLeftChildPosition(int index)
        {
            return index * 2;
        }

        private int GetParentPositionInHeap(int position)
        {
            return position / 2;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Huffman.Legacy
{
    internal sealed class PriorityQueue<T>
    {
        private readonly List<T> _heap;
        private readonly IComparer<T> _comparer;

        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new List<T>();
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public int Count => _heap.Count;

        public void Enqueue(T item)
        {
            _heap.Add(item);
            HeapifyUp(_heap.Count - 1);
        }

        public T Dequeue()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            T firstItem = _heap[0];
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);

            if (_heap.Count > 0)
            {
                HeapifyDown(0);
            }
            return firstItem;
        }

        public T Peek()
        {
            if (_heap.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            return _heap[0];
        }

        public void Clear()
        {
            _heap.Clear();
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_comparer.Compare(_heap[index], _heap[parentIndex]) >= 0)
                {
                    break;
                }
                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int current = index;
            while (true)
            {
                int smallest = current;
                int leftChild = 2 * current + 1;
                int rightChild = 2 * current + 2;

                if (leftChild < _heap.Count &&
                    _comparer.Compare(_heap[leftChild], _heap[smallest]) < 0)
                {
                    smallest = leftChild;
                }
                if (rightChild < _heap.Count &&
                    _comparer.Compare(_heap[rightChild], _heap[smallest]) < 0)
                {
                    smallest = rightChild;
                }

                if (smallest == current)
                    break;

                Swap(current, smallest);
                current = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
        }
    }
}
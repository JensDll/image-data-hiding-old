using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.DataStructures
{
    public class Heap<T>
    {
        private const int INIT_SIZE = 8;

        private int next = 0;
        private T[] heap = new T[INIT_SIZE];
        private readonly Comparison<T> comparison;

        public int Count => next;

        public Heap(Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        public Heap(Comparison<T> comparison, IEnumerable<T> values)
        {
            this.comparison = comparison;
            Heapify(values.ToArray());
        }

        public Heap<T> Add(T item)
        {
            if (next >= heap.Length)
            {
                DoubleHeapSize();
            }

            heap[next] = item;
            SiftUp(next++);

            return this;
        }

        public T Peek() => heap[0];

        public T RemoveTop()
        {
            T root = heap[0];
            heap[0] = heap[next - 1];
            heap[next - 1] = default;

            if (next >= INIT_SIZE && next <= heap.Length / 2)
            {
                BisectHeapSize();
            }

            next--;
            SiftDown(0);

            return root;
        }

        private void SiftUp(int i)
        {
            if (i == 0)
            {
                return;
            }

            int parentIndex = ParentIndex(i);

            T current = heap[i];
            T parent = heap[parentIndex];

            if (comparison(current, parent) < 0)
            {
                Swap(i, parentIndex);
                SiftUp(parentIndex);
            }
        }

        private void SiftDown(int i)
        {
            int leftChildIndex = LeftChildIndex(i);
            int rightChildIndex = RightChildIndex(i);

            int smallestIndex = i;
            T smallest = heap[i];

            if (leftChildIndex < next)
            {
                T leftChild = heap[leftChildIndex];

                if (comparison(leftChild, smallest) < 0)
                {
                    smallestIndex = leftChildIndex;
                    smallest = leftChild;
                }
            }

            if (rightChildIndex < next)
            {
                T rightChild = heap[rightChildIndex];

                if (comparison(rightChild, smallest) < 0)
                {
                    smallestIndex = rightChildIndex;
                }
            }

            if (i != smallestIndex)
            {
                Swap(i, smallestIndex);
                SiftDown(smallestIndex);
            }
        }

        private void Heapify(T[] values)
        {
            int size = INIT_SIZE;

            while (size < values.Length)
            {
                size <<= 1;
            }

            next = values.Length;
            heap = new T[size];
            values.CopyTo(heap, 0);

            int indexLastNonLeave = (values.Length / 2) - 1;

            for (int i = indexLastNonLeave; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        private void DoubleHeapSize()
        {
            var newHeap = new T[heap.Length * 2];
            heap.CopyTo(newHeap, 0);
            heap = newHeap;
        }

        private void BisectHeapSize()
        {
            int newLength = heap.Length / 2;
            heap = heap[0..newLength];
        }

        private static int ParentIndex(int i) => (i - 1) / 2;

        private static int LeftChildIndex(int i) => 2 * i + 1;

        private static int RightChildIndex(int i) => 2 * i + 2;
    }
}

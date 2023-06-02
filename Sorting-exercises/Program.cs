using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Exercise_3
{
    class Program
    {
        class KeyValuePair
        {
            public string Key { get; set; }

            public int Value { get; set; }

            public override string ToString()
            {
                return $"{Key} : {Value}";
            }
        }

        class Position
        {
            public KeyValuePair KeyValuePair { get; set; }

            public Position Next { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append($"{KeyValuePair}");
                var next = Next;
                while (next != null)
                {
                    sb.Append($";{next.KeyValuePair}");
                    next = next.Next;
                }

                return sb.ToString();
            }
        }

        class WordFrequencyTable
        {
            private readonly Position[] _positions;

            public WordFrequencyTable(int capacity)
            {
                _positions = new Position[capacity];
            }

            private int HashFunc(string key)
            {
                // var v = 0;
                // foreach (var c in key)
                // {
                //     v += c;
                // }
                //
                // return v % _positions.Length;

                var v = 0;
                foreach (var c in key)
                {
                    v = (v << 5 + c) % _positions.Length;
                }

                return v;
            }

            public KeyValuePair Add(string key)
            {
                var p = HashFunc(key);

                var position = _positions[p];

                if (position is null)
                {
                    position = new Position();
                    _positions[p] = position;
                }
                else
                {
                    while (position.Next != null)
                    {
                        position = position.Next;
                    }

                    position.Next = new Position();
                    position = position.Next;
                }

                position.KeyValuePair = new KeyValuePair
                {
                    Key = key
                };

                return position.KeyValuePair;
            }

            public void Remove(string key)
            {
                var p = HashFunc(key);

                var position = _positions[p];
                if (position.KeyValuePair.Key == key)
                {
                    _positions[p] = position.Next;
                }
                else
                {
                    while (position.Next != null && position.Next.KeyValuePair.Key != key)
                    {
                        position = position.Next;
                    }

                    if (position.Next.KeyValuePair.Key == key)
                    {
                        position.Next = position.Next?.Next;
                    }
                }
            }

            private KeyValuePair GetKVP(string key)
            {
                var p = HashFunc(key);

                var position = _positions[p];

                if (position is null)
                {
                    return null;
                }

                while (position != null && position.KeyValuePair.Key != key)
                {
                    position = position.Next;
                }

                return position?.KeyValuePair;
            }

            public int this[string key]
            {
                get
                {
                    var kvp = GetKVP(key);

                    return kvp?.Value ?? 0;
                }
                set
                {
                    var kvp = GetKVP(key);

                    if (value > 0 && kvp is null)
                    {
                        kvp = Add(key);
                    }
                    else if (value <= 0)
                    {
                        Remove(key);
                        return;
                    }

                    kvp.Value = value;
                }
            }

            public override string ToString() =>
                _positions
                    .Where(x => x != null)
                    .Select(x => $"{x}")
                    .Aggregate((f, s) => $"{f}{Environment.NewLine}{s}");
        }

        public static void Main(string[] args)
        {
            #region Sorting

            var r = new Random();

            var arr = Enumerable.Range(0, 20000)
                .Select(s => r.Next())
                .ToArray();

            #region MergeSort

            var arr1 = arr.ToArray();
            var time1 = DateTime.Now;
            MergeSort(arr1, 0, arr1.Length - 1);
            var time2 = DateTime.Now;
            Console.WriteLine($"Merge sort: {(time2 - time1).Milliseconds}");

            #endregion

            #region Insertion sort

            var arr2 = arr.ToArray();

            var time3 = DateTime.Now;
            InsertionSort(arr2);
            var time4 = DateTime.Now;

            Console.WriteLine($"Insertion sort: {(time4 - time3).Milliseconds}");

            #endregion

            #region Heap sort

            var arr3 = arr.ToArray();
            var time5 = DateTime.Now;
            HeapSort(arr3);
            var time6 = DateTime.Now;
            Console.WriteLine($"Heap sort: {(time6 - time5).Milliseconds}");

            #endregion

            #endregion

            var wordFrequencyTable = new WordFrequencyTable(100);

            Console.Write("Enter text: ");
            var words = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                wordFrequencyTable[word]++;
            }

            Console.WriteLine(wordFrequencyTable);
        }

        private static void Merge(IList<int> arr, int l, int m, int r)
        {
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;
            var L = new int[n1];
            for (i = 0; i < n1; i++)
            {
                L[i] = arr[l + i];
            }

            var R = new int[n2];
            for (j = 0; j < n2; j++)
            {
                R[j] = arr[m + 1 + j];
            }

            i = j = 0;
            k = l;
            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                {
                    arr[k] = L[i++];
                }
                else
                {
                    arr[k] = R[j++];
                }

                k++;
            }

            while (i < n1)
            {
                arr[k++] = L[i++];
            }

            while (j < n2)
            {
                arr[k++] = R[j++];
            }
        }

        private static void MergeSort(IList<int> arr, int l, int r)
        {
            if (l < r)
            {
                int m = l + (r - l) / 2;
                MergeSort(arr, l, m);
                MergeSort(arr, m + 1, r);

                Merge(arr, l, m, r);
            }
        }

        private static void InsertionSort(IList<int> arr)
        {
            int n = arr.Count;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }

                arr[j + 1] = key;
            }
        }

        private static void HeapSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(arr, n, i);
            }

            for (int i = n - 1; i > 0; i--)
            {
                (arr[0], arr[i]) = (arr[i], arr[0]);

                Heapify(arr, i, 0);
            }
        }

        private static void Heapify(IList<int> arr, int n, int i)
        {
            var largest = i;
            var l = 2 * i + 1;
            var r = 2 * i + 2;

            if (l < n && arr[l] > arr[largest])
                largest = l;

            if (r < n && arr[r] > arr[largest])
                largest = r;

            if (largest != i)
            {
                (arr[i], arr[largest]) = (arr[largest], arr[i]);

                Heapify(arr, n, largest);
            }
        }
    }
}

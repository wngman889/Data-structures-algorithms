using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingExercise
{
    public class Program
    {
        #region exercise 2 variables

        private const int Capacity = 3;
        private static Queue<object> queue = new Queue<object>(Capacity);
        private static Semaphore semaphoreFreeSpace = new Semaphore(Capacity, Capacity);
        private static Semaphore semaphoreBread = new Semaphore(0, Capacity);

        #endregion

        #region exercise 3 variables

        private static Semaphore[] forks;

        private const int Count = 5;

        #endregion

        public static void Main()
        {
            #region exercise 1

            // var arr = Enumerable.Range(1, 1_000_000)
            //     .OrderBy(x => Guid.NewGuid().ToString())
            //     .ToArray();
            //
            // var dt1 = DateTime.Now;
            // Console.WriteLine($"{FindMaxElement(arr, 0, arr.Length)}");
            // var dt2 = DateTime.Now;
            // Console.WriteLine($"{(dt2 - dt1).Milliseconds}");
            //
            // var dt3 = DateTime.Now;
            // FindMaxElementParallel(arr, 4);
            // var dt4 = DateTime.Now;
            // Console.WriteLine($"{(dt4 - dt3).Milliseconds}");

            #endregion

            #region exercise 2

            // new Thread(Baker).Start();
            // new Thread(Client).Start(1);
            // new Thread(Client).Start(2);
            // new Thread(Client).Start(3);

            #endregion

            #region exercise 3

            // forks = Enumerable.Range(0, 5)
            //     .Select(s => new Semaphore(1, 1))
            //     .ToArray();
            //
            // new Thread(Student).Start(0);
            // new Thread(Student).Start(1);
            // new Thread(Student).Start(2);
            // new Thread(Student).Start(3);
            // new Thread(Student).Start(4);

            #endregion
        }

        #region exercise 1 methods

        private static int FindMaxElementParallel(int[] arr, int threadsCount)
        {
            var threads = new Thread[threadsCount];
            var maxValues = new int[threadsCount];
            var part = arr.Length / threadsCount;

            for (int t = 0; t < threads.Length; t++)
            {
                threads[t] = new Thread((obj) =>
                {
                    var _t = (int)obj;
                    maxValues[_t] = FindMaxElement(arr, _t * part, (_t + 1) * part);
                });

                threads[t].Start(t);
            }

            for (int t = 0; t < threadsCount; t++)
            {
                threads[t].Join();
            }

            return FindMaxElement(maxValues, 0, maxValues.Length);
        }

        private static int FindMaxElement(int[] arr, int l, int r)
        {
            int max = arr[0];
            for (int i = l + 1; i < r; i++)
            {
                if (arr[i] > max)
                {
                    max = arr[i];
                }
            }

            return max;
        }

        #endregion

        #region exercise 2 methods

        private static void Baker()
        {
            var r = new Random();

            while (true)
            {
                semaphoreFreeSpace.WaitOne();

                lock (queue)
                {
                    Thread.Sleep(r.Next(100, 100));

                    Console.WriteLine($"Baker: making bread {queue.Count + 1}");

                    queue.Enqueue(null);
                    semaphoreBread.Release();
                }
            }
        }

        // private static void Client(object obj)
        // {
        //     var n = (int) obj;
        //     var r = new Random();
        //
        //     while (true)
        //     {
        //         Thread.Sleep(r.Next(600, 700));
        //
        //         semaphoreBread.WaitOne();
        //         lock (queue)
        //         {
        //             Console.WriteLine($"Client {n}: buying bread {queue.Count}");
        //
        //             queue.Dequeue();
        //             semaphoreFreeSpace.Release();
        //         }
        //     }
        // }

        #endregion

        #region exercise 3 methods

        static void Student(object obj)
        {
            var n = (int)obj;
            var r = new Random();
            while (true)
            {
                Console.WriteLine($"Student {n} starts thinking");
                Thread.Sleep(r.Next(2000, 3000));

                forks[n].WaitOne();
                forks[(n + 1) % Count].WaitOne();

                Console.WriteLine($"Student {n} starts eating");
                Thread.Sleep(r.Next(2000, 3000));
                Console.WriteLine($"Student {n} stops eating");
                forks[n].Release();
                forks[(n + 1) % Count].Release();
            }
        }

        #endregion

    }
}

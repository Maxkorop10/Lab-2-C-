using System;
using System.Threading;

namespace CSharp_task
{
    class Program
    {
        private static readonly int dim = 100;
        private static readonly int threadNum = 4;
        private static readonly object locker = new object();

        private Thread[] threads = new Thread[threadNum];
        private int[] arr = new int[dim];
        private int min_index = -1;
        private int min_element;

        static void Main(string[] args)
        {
            Program main = new Program();
            main.InitArr();
            main.Parallel_Min();
            Console.WriteLine("\nMinimal element: " + main.min_element);
            Console.WriteLine("Index of minimal element: " + main.min_index + "\n");
            Console.ReadKey();
        }

        private void InitArr()
        {
            for (int i = 0; i < dim; i++) { arr[i] = i; }

            arr[2] = -10;
        }

        private void Parallel_Min()
        {
            int part_size = dim / threadNum;

            for (int i = 0; i < threadNum; i++)
            {
                int startIndex = i * part_size;
                int finishIndex = (i + 1) * part_size;
                if (i == threadNum - 1) { finishIndex = dim; }
                threads[i] = new Thread(() => Find_Min(startIndex, finishIndex));
                threads[i].Start();
            }

            for (int i = 0; i < threadNum; i++) { threads[i].Join(); }
        }

        private void Find_Min(int startIndex, int finishIndex)
        {
            int min = int.MaxValue;
            int index = -1;

            for (int i = startIndex; i < finishIndex; i++)
            {
                if (arr[i] < min)
                {
                    min = arr[i];
                    index = i;
                }
            }

            lock (locker)
            {
                if (min < min_element)
                {
                    min_element = min;
                    min_index = index;
                }
            }
        }
    }
}

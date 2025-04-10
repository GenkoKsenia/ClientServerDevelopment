using System;
using System.Threading;

namespace LabPart1
{
    class Lab1
    {
        static void Main1()
        {
            // Параметры для первого потока
            int start1 = 1;
            int end1 = 5;

            // Параметры для второго потока
            int start2 = 10;
            int end2 = 15;

            // Создание потоков с передачей параметров через лямбда-выражение
            Thread thread1 = new Thread(() => PrintNumbers(start1, end1));
            Thread thread2 = new Thread(() => PrintNumbers(start2, end2));

            // Запуск потоков
            thread1.Start();
            thread2.Start();

            // Ожидание завершения потоков
            thread1.Join();
            thread2.Join();

            Console.WriteLine("Все потоки завершили работу!");
        }

        // Метод для вывода чисел в диапазоне
        static void PrintNumbers(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                Console.WriteLine(
                    $"Поток [{Thread.CurrentThread.ManagedThreadId}]: {i} " + 
                    $"({DateTime.Now:HH:mm:ss.fff})"
                );
                Thread.Sleep(100); // Имитация работы
            }
        }
    }
}

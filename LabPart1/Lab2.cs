using System;
using System.Threading;

namespace LabPart1
{
    class Lab2
    {
        static ManualResetEvent waitHandle = new ManualResetEvent(false);

        static void Main2()
        {
            Console.WriteLine("=== Эксперимент 1: Запуск первого потока первым ===");
            RunExperiment(startSecondThreadFirst: false);

            Console.WriteLine("\n=== Эксперимент 2: Запуск второго потока первым с задержкой ===");
            RunExperiment(startSecondThreadFirst: true);

            Console.WriteLine("\nВсе эксперименты завершены!");
        }

        static void RunExperiment(bool startSecondThreadFirst)
        {
            waitHandle.Reset(); // Сбрасываем событие в несигнальное состояние

            Thread thread1 = new Thread(PrintNumbersFirst);
            Thread thread2 = new Thread(PrintNumbersSecond);

            if (startSecondThreadFirst)
            {
                // Запускаем второй поток первым с задержкой
                thread2.Start();
                Thread.Sleep(1000); // Задержка 1 секунда
                thread1.Start();
            }
            else
            {
                // Стандартный запуск
                thread1.Start();
                thread2.Start();
            }

            thread1.Join();
            thread2.Join();
        }

        // Первый поток
        static void PrintNumbersFirst()
        {
            Console.WriteLine($"Первый поток [{Thread.CurrentThread.ManagedThreadId}] начал работу");
            for (int i = 1; i <= 100; i++)
            {
                Console.Write($"[1:{i}] ");
            }
            Console.WriteLine($"\nПервый поток [{Thread.CurrentThread.ManagedThreadId}] завершил работу");
            waitHandle.Set(); // Сигнализируем о завершении
        }

        // Второй поток
        static void PrintNumbersSecond()
        {
            Console.WriteLine($"Второй поток [{Thread.CurrentThread.ManagedThreadId}] начал ожидание");
            waitHandle.WaitOne(); // Ожидаем сигнал от первого потока
            Console.WriteLine($"\nВторой поток [{Thread.CurrentThread.ManagedThreadId}] начал работу");
            for (int i = 1; i <= 100; i++)
            {
                Console.Write($"[2:{i}] ");
            }
            Console.WriteLine($"\nВторой поток [{Thread.CurrentThread.ManagedThreadId}] завершил работу");
        }
    }
}
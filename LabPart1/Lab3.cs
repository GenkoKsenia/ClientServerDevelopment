namespace LabPart1
{
    class Lab3
    {
        static double sharedValue = 0.5; // Начальное значение (-1 ≤ value ≤ 1 для корректного Arccos)
        static readonly object lockObj = new object();
        static AutoResetEvent cosReady = new AutoResetEvent(false);
        static AutoResetEvent arccosReady = new AutoResetEvent(false);
        static bool running = true;

        static void Main()
        {
            Thread cosThread = new Thread(CalculateCos);
            Thread arccosThread = new Thread(CalculateArccos);

            cosThread.Start();
            arccosThread.Start();

            // Дадим потокам поработать 5 секунд
            Thread.Sleep(5000);
            running = false;

            cosThread.Join();
            arccosThread.Join();
        }

        static void CalculateCos()
        {
            while (running)
            {
                lock (lockObj)
                {
                    double result = Math.Cos(sharedValue);
                    Console.WriteLine($"Cos({sharedValue:F4}) = {result:F4}");
                    sharedValue = result;
                }
                
                // Сигнал для потока с Arccos и ожидание ответного сигнала
                arccosReady.Set();
                cosReady.WaitOne();
            }
        }

        static void CalculateArccos()
        {
            arccosReady.WaitOne(); // Начальное ожидание
            
            while (running)
            {
                lock (lockObj)
                {
                    if (Math.Abs(sharedValue) > 1.0)
                    {
                        Console.WriteLine("Ошибка: значение вне диапазона Arccos!");
                        return;
                    }
                    
                    double result = Math.Acos(sharedValue);
                    Console.WriteLine($"Arccos({sharedValue:F4}) = {result:F4}");
                    sharedValue = result;
                }
                
                // Сигнал для потока с Cos и ожидание следующего сигнала
                cosReady.Set();
                arccosReady.WaitOne();
            }
        }
    }
}
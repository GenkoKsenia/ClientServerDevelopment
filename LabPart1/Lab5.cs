using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LabPart1
{
    class Lab5
    {
        class Grade
        {
            public string StudentName { get; set; }
            public string Subject { get; set; }
            public int Score { get; set; }
        }

        static async Task Main()
        {        
            List<Grade> grades = new List<Grade>
            {
                new Grade { StudentName = "Вася", Subject = "Математика", Score = 90 },
                new Grade { StudentName = "Вася", Subject = "Физика", Score = 85 },
                new Grade { StudentName = "Петя", Subject = "Математика", Score = 75 },
                new Grade { StudentName = "Петя", Subject = "Физика", Score = 80 },
                new Grade { StudentName = "Коля", Subject = "Математика", Score = 95 },
                new Grade { StudentName = "Коля", Subject = "Физика", Score = 90 }
            };

            // Синхронный расчёт
            var sw = Stopwatch.StartNew();
            var syncAvg = CalculateAverageSync(grades);
            sw.Stop();
            Console.WriteLine($"Однопоточный метод: {sw.ElapsedTicks} тиков | Среднее: {syncAvg:F2}");

            // Асинхронный расчёт
            sw.Restart();
            var asyncAvg = await CalculateAverageAsync(grades);
            sw.Stop();
            Console.WriteLine($"Многопоточный метод: {sw.ElapsedTicks} тиков | Среднее: {asyncAvg:F2}");

            Console.WriteLine($"Результаты совпадают: {Math.Abs(syncAvg - asyncAvg) < 0.001}");
        }

        static double CalculateAverageSync(List<Grade> grades)
        {
            return grades.Average(g => g.Score);
        }

        static async Task<double> CalculateAverageAsync(List<Grade> grades)
        {
            // Разделяем данные на 2 части для параллельной обработки
            var parts = grades
                .Select((g, index) => new { g, index })
                .GroupBy(x => x.index % 2)
                .Select(g => g.Select(x => x.g).ToList())
                .ToList();

            var tasks = parts.Select(part => 
                Task.Run(() => part.Sum(g => g.Score))
            ).ToList();

            var sums = await Task.WhenAll(tasks);
            return (double)sums.Sum() / grades.Count;
        }
    }
}
    


using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using FEZAnalyzer.WarRecognize;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FEZAnalyzer.PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //PerformSkillCollection().Wait();
            PerformResultImage();
        }

        private static async Task PerformSkillCollection()
        {
            //using (var m = new Mat("katate.png"))
            using (var m = new Mat("koori.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var recognizer = await SkillCollectionRecognizer.CreateAsync();

                long maxCount = -1;
                long max = long.MinValue;
                long min = long.MaxValue;
                long total = 0;
                const int count = 1000;
                for (int i = 0; i < count; i++)
                {
                    var sw = Stopwatch.StartNew();
                    recognizer.Recognize(img);
                    sw.Stop();
                    total += sw.ElapsedMilliseconds;
                    if (max < sw.ElapsedMilliseconds)
                    {
                        max = sw.ElapsedMilliseconds;
                        maxCount = i;
                    }
                    min = Math.Min(min, sw.ElapsedMilliseconds);
                }

                Console.WriteLine($"Avg:{(double)total / count}ms");
                Console.WriteLine($"Max:{max}ms count:{maxCount}");
                Console.WriteLine($"Min:{min}ms");
                Console.ReadKey();
            }
        }

        private static void PerformResultImage()
        {
            using (var m = new Mat("Score.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var recognizer = new NewUiWarScoreRecognizer();

                long maxCount = -1;
                long max = long.MinValue;
                long min = long.MaxValue;
                long total = 0;
                const int count = 10000;
                List<long> list = new List<long>();
                for (int i = 0; i < count; i++)
                {
                    var sw = Stopwatch.StartNew();
                    recognizer.Recognize(img);
                    sw.Stop();

                    list.Add(sw.ElapsedMilliseconds);

                    if (i != 0)
                    {
                        total += sw.ElapsedMilliseconds;
                        if (max < sw.ElapsedMilliseconds)
                        {
                            max = sw.ElapsedMilliseconds;
                            maxCount = i;
                        }
                        min = Math.Min(min, sw.ElapsedMilliseconds);
                    }
                }

                var top10 = list.OrderByDescending(x => x).Take(10);

                Console.WriteLine($"Avg:{(double)total / count}ms");
                Console.WriteLine($"Max:{max}ms count:{maxCount}");
                Console.WriteLine($"Min:{min}ms");
                Console.WriteLine("");
                Console.WriteLine(string.Join("\r\n", top10));
                Console.ReadKey();
            }
        }
    }
}

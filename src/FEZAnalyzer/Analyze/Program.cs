using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using OpenCvSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analyze
{
    class Program
    {
        static void Main(string[] args)
        {
            var scores = new ConcurrentBag<WarScore>();

            var files1 = Directory.GetFiles(@"D:\data\Picture\fez_screenshot");
            var files2 = Directory.GetFiles(@"E:\picture\fez_screenshot");
            var files  = Enumerable.Concat(files1, files2).ToArray();

            var count = 0;
            var error = 0;
            Parallel.ForEach(files, f =>
            //foreach (var f in files)
            {
                try
                {
                    var c = Interlocked.Increment(ref count);

                    using (var m = new Mat(f))
                    using (var mat = new Mat<Vec3b>(m))
                    using (var img = new FezImage(mat))
                    {
                        var r = new NewUiWarScoreRecognizer();
                        var s = r.Recognize(img);
                        scores.Add(s);
                    }

                    if (c % 10 == 0) Console.WriteLine($"{c}/{files.Length}");
                }
                catch
                {
                    Interlocked.Increment(ref error);
                }
            }
            );

            Console.WriteLine($"Error:{error}");

            var g = scores
                .GroupBy(x =>
                {
                    if (x.参戦側 == Side.Offence)
                        return new { Side = x.参戦側, Country = x.攻撃側国 };
                    else
                        return new { Side = x.参戦側, Country = x.防衛側国 };
                })
                .OrderBy(x => x.Key.Country).ThenBy(x => x.Key.Side)
                .Select(x =>
                {
                    var total = (double)x.Count();
                    var win   = x.Where(y => y.勝敗 == WarResult.Win).Count();
                    var lose  = x.Where(y => y.勝敗 == WarResult.Lose).Count();

                    return $"{x.Key.Side},{x.Key.Country},{win},{lose},{total},{win / total * 100:0.0}%";
                });

            Console.WriteLine(string.Join(Environment.NewLine, g));
        }
    }
}

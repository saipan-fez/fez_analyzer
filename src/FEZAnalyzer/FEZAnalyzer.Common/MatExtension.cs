using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace OpenCvSharp
{
    public static class MatExtension
    {
        public static string CalculateHash(this Mat mat)
        {
            using (var sha1 = SHA1.Create())
            {
                var bytes = new byte[mat.Total()];
                Marshal.Copy(mat.Data, bytes, 0, bytes.Length);

                var hash = sha1.ComputeHash(bytes);

                return string.Join(",", hash);
            }
        }

        public static bool CompareAsRgbColor(this Mat<Vec3b> mat1, Mat<Vec3b> mat2, int threshold)
        {
            // OpenCVの関数を使っての比較では画像全体を対象とせざるを得ないため速度が遅い。
            // そのため、メモリ内を順に比較する方法を採る。
            // 
            // 参考情報
            // OpenCV版： Avg 13.5ms, Max 96ms, Min 1ms 
            // 独自版　： Avg  0.1ms, Max 31ms, Min 0ms

            // -------------
            // OpenCV版
            // -------------
            //using (var diff = new Mat())
            //using (var range = new Mat())
            //{
            //    Cv2.Absdiff(mat1, mat2, diff);
            //    Cv2.InRange(diff, new Scalar(0, 0, 0), new Scalar(threshold, threshold, threshold), range);
            //    return Cv2.CountNonZero(range) == range.Cols * range.Rows * range.Channels();
            //}

            // -------------
            // 独自版
            // -------------
            if (mat1.Cols != mat2.Cols || mat1.Rows != mat2.Rows || mat1.Dims != mat2.Dims)
            {
                return false;
            }

            var indexer1 = mat1.GetIndexer();
            var indexer2 = mat2.GetIndexer();

            for (int y = 0; y < mat1.Height; y++)
            {
                for (int x = 0; x < mat1.Width; x++)
                {
                    var d0 = indexer1[y, x].Item0 - indexer2[y, x].Item0;
                    var d1 = indexer1[y, x].Item1 - indexer2[y, x].Item1;
                    var d2 = indexer1[y, x].Item2 - indexer2[y, x].Item2;
                    if (Math.Abs(d0) > threshold ||
                        Math.Abs(d1) > threshold ||
                        Math.Abs(d2) > threshold)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void Threshold(this Mat<Vec3b> mat)
        {
            var white = new Vec3b(0xff, 0xff, 0xff);
            var black = new Vec3b(0x00, 0x00, 0x00);

            var indexer = mat.GetIndexer();
            for (int y = 0; y < mat.Height; y++)
            {
                for (int x = 0; x < mat.Width; x++)
                {
                    if (indexer[y, x] != white)
                    {
                        indexer[y, x] = black;
                    }
                }
            }
        }
    }
}

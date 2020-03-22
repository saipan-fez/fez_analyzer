using FEZAnalyzer.Entity;
using FEZAnalyzer.ResourceProvider;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    internal class PowRecognizer : IWarRecognize<int>
    {
        public const int InvalidPow = int.MaxValue;

        private IReadOnlyDictionary<string, int> _table = null;

        private PowRecognizer()
        { }

        public static async Task<PowRecognizer> CreateAsync()
        {
            var powRecognizer = new PowRecognizer()
            {
                _table = await PowHashTableProvider.ProvideAsync()
            };

            return powRecognizer;
        }

        public int Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var pow = InvalidPow;

            var m = fezImage.Image;
            var w = m.Width;
            var h = m.Height;

            // 一桁ごとに数字を解析し、全桁解析できれば数値に変換する
            using (var houndredsPlace = m.Clone(new Rect(w - 143, h - 67, 7, 10)))     // 百の位
            using (var tensPlace      = m.Clone(new Rect(w - 135, h - 67, 7, 10)))     // 十の位
            using (var onesPlace      = m.Clone(new Rect(w - 127, h - 67, 7, 10)))     // 一の位
            {
                var houndreds = GetPowNum(houndredsPlace);
                var tens      = GetPowNum(tensPlace);
                var ones      = GetPowNum(onesPlace);

                if (houndreds != InvalidPow &&
                    tens      != InvalidPow &&
                    ones      != InvalidPow)
                {
                    pow = houndreds * 100 + tens * 10 + ones;
                }
            }

            return pow;
        }

        private int GetPowNum(Mat mat)
        {
            var hash = mat.CalculateHash();
            if (_table.TryGetValue(hash, out int pow))
            {
                return pow;
            }

            // 上記に当てはまらないものはエラーとする
            return InvalidPow;
        }
    }
}

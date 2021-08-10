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
    internal class HpRecognizer : IWarRecognize<int>
    {
        public const int InvalidValue = int.MaxValue;

        private IReadOnlyDictionary<string, int> _table = null;

        private HpRecognizer()
        { }

        public static async Task<HpRecognizer> CreateAsync()
        {
            var powRecognizer = new HpRecognizer()
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

            return GetHp(fezImage);
        }

        private int GetHp(FezImage fezImage)
        {
            var hp = InvalidValue;

            var m = fezImage.Image;
            var w = m.Width;
            var h = m.Height;

            using (var thousandsPlace = m.Clone(new Rect(w - 151, h - 82, 7, 10)))     // 千の位
            using (var houndredsPlace = m.Clone(new Rect(w - 143, h - 82, 7, 10)))     // 百の位
            using (var tensPlace      = m.Clone(new Rect(w - 135, h - 82, 7, 10)))     // 十の位
            using (var onesPlace      = m.Clone(new Rect(w - 127, h - 82, 7, 10)))     // 一の位
            {
                var thousands = GetNum(thousandsPlace);
                var houndreds = GetNum(houndredsPlace);
                var tens      = GetNum(tensPlace);
                var ones      = GetNum(onesPlace);

                if (thousands != int.MaxValue &&
                    houndreds != int.MaxValue &&
                    tens      != int.MaxValue &&
                    ones      != int.MaxValue)
                {
                    hp = thousands * 1000 + houndreds * 100 + tens * 10 + ones;
                }
            }

            return hp;
        }

        private int GetNum(Mat mat)
        {
            var hash = mat.CalculateHash();
            if (_table.TryGetValue(hash, out int num))
            {
                return num;
            }

            // 上記に当てはまらないものはエラーとする
            return InvalidValue;
        }
    }
}

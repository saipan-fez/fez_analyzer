﻿using FEZAnalyzer.Entity;
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
        public const int InvalidValue = int.MaxValue;

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

            return GetPow(fezImage);
        }

        private int GetPow(FezImage fezImage)
        {
            var pow = InvalidValue;

            var m = fezImage.Image;
            var w = m.Width;
            var h = m.Height;

            // 一桁ごとに数字を解析し、全桁解析できれば数値に変換する
            using (var houndredsPlace = m.Clone(new Rect(w - 143, h - 67, 7, 10)))     // 百の位
            using (var tensPlace      = m.Clone(new Rect(w - 135, h - 67, 7, 10)))     // 十の位
            using (var onesPlace      = m.Clone(new Rect(w - 127, h - 67, 7, 10)))     // 一の位
            {
                var houndreds = GetNum(houndredsPlace);
                var tens      = GetNum(tensPlace);
                var ones      = GetNum(onesPlace);

                if (houndreds != InvalidValue &&
                    tens      != InvalidValue &&
                    ones      != InvalidValue)
                {
                    pow = houndreds * 100 + tens * 10 + ones;
                }
            }

            return pow;
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

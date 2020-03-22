using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// Powデバフ解析クラス
    /// </summary>
    internal class PowDebuffArrayRecognizer : IWarRecognize<PowDebuff[]>
    {
        /// <summary>
        /// 右下アイコンの最大個数
        /// </summary>
        /// <remarks>
        /// 最大個数は未調査。
        /// この数までバフが増えないだろうという想定での個数。
        /// </remarks>
        private const int MaxPowerDebuffCount = 10;

        /// <summary>
        /// L*a*b色空間上での色差の閾値
        /// </summary>
        private const double Threshold = 10.0d;

        // パワーブレイクかどうか判定する色
        private readonly Vec3b PowerBreakCmpColor1 = new Vec3b(183, 125, 30);
        private readonly Vec3b PowerBreakCmpColor2 = new Vec3b(187, 134, 49);

        // パワーブレイク
        //   Lv1-3で減少Powが異なる
        private readonly PowDebuff PowerBreak =
            new PowDebuff("パワーブレイク", new int[]{ -15, -20, -25 }, 8, TimeSpan.FromSeconds(3));

        public PowDebuff[] Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var w = fezImage.Image.Width;
            var h = fezImage.Image.Height;

            // バフ・デバフは右下に順不同で並ぶため、右端から最大数まで順に判定する
            var indexer = fezImage.Image.GetIndexer();
            for (var i = 0; i < MaxPowerDebuffCount; i++)
            {
                var x1 = w - 28 - (32 * i);
                var x2 = w - 34 - (32 * i);
                var y  = h - 20;

                // 右下のアイコンは透過が少しあるため、ピクセル単位の色が完全一致しない。
                // そのため、L*a*b色空間での色差が閾値以内かどうかで判定する。
                var compare = true;
                compare &= ColorDiff.CompareAsLabColor(indexer[y, x1], PowerBreakCmpColor1, Threshold);
                compare &= ColorDiff.CompareAsLabColor(indexer[y, x2], PowerBreakCmpColor2, Threshold);

                if (compare)
                {
                    return new PowDebuff[]
                    {
                        PowerBreak
                    };
                }
            }

            return new PowDebuff[0];
        }
    }
}

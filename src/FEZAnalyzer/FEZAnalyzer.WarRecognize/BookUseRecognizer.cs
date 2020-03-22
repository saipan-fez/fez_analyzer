using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// 書使用解析クラス
    /// </summary>
    /// <remarks>
    /// 精練の密書などの書が使用されているか解析する。
    /// ただし、書の時間や回数書・時間書の判断はできず、
    /// 何らかの書が1種類でも使用されているかどうかのみ判断する。
    /// </remarks>
    internal class BookUseRecognizer : IWarRecognize<bool>
    {
        /// <summary>
        /// L*a*b色空間上での色差の閾値
        /// </summary>
        /// <remarks>
        /// 若干透過が入っているためか色差を大きめにとる
        /// (10.0dでは一部超える場合があった)
        /// </remarks>
        private const double Threshold = 15.0d;

        public bool Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var ret = true;
            var mat = fezImage.Image;
            var h   = mat.Height;
            var w   = mat.Width;
            var idx = mat.GetIndexer();

            // 右下の書使用アイコンが表示されているか、3箇所の色から判断する
            // ※書使用アイコンは位置固定
            ret &= ColorDiff.CompareAsLabColor(idx[h - 27, w - 432], new Vec3b( 15, 135, 255), Threshold);
            ret &= ColorDiff.CompareAsLabColor(idx[h - 27, w - 431], new Vec3b(214, 255, 255), Threshold);
            ret &= ColorDiff.CompareAsLabColor(idx[h - 10, w - 449], new Vec3b(  3, 255, 255), Threshold);

            return ret;
        }
    }
}

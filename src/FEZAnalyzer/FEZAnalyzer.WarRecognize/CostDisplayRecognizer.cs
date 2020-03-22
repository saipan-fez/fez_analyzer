using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// コスト表示有無の解析クラス
    /// </summary>
    public class CostDisplayRecognizer : IWarRecognize<bool>
    {
        private static readonly Vec3b colorCost  = new Vec3b(123, 123, 123);
        private static readonly Vec3b colorSlash = new Vec3b(156, 156, 156);

        public bool Recognize(FezImage fezImage)
        {
            if (fezImage == null)
            {
                throw new ArgumentNullException();
            }

            var ret = true;
            var mat = fezImage.Image;
            var w   = mat.Width;
            var h   = mat.Height;

            // [Cost 150 / 150] の"C"と"/"の部分で判別する
            var indexer = mat.GetIndexer();
            ret &= indexer[h - 128, w - 441] == colorCost;   // "C"
            ret &= indexer[h - 120, w - 375] == colorSlash;  // "/"

            return ret;
        }
    }
}

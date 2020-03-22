using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class WarResultScanner
    {
        private const int Threshold = 3;

        // 戦争結果判定用の座標と色
        private readonly Point WarResultPoint = new Point(250, 50);
        private readonly Vec3b WarResultLoseColor = new Vec3b(52, 52, 52);

        public WarResult Scan(WarResultImage warResultImage)
        {
            if (warResultImage == null)
            {
                throw new ArgumentNullException();
            }

            var color = warResultImage.Image.At<Vec3b>(WarResultPoint.Y, WarResultPoint.X);

            return ColorDiff.CompareAsBgrColor(color, WarResultLoseColor, Threshold) ? WarResult.Lose : WarResult.Win;
        }
    }
}

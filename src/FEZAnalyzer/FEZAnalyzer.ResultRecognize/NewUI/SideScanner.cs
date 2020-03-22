using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class SideScanner
    {
        private const int Threshold = 3;

        // 攻守判定用の座標と色
        private readonly Point WarSidePoint = new Point(200, 100);
        private readonly Vec3b WarSideWinDefenseColor  = new Vec3b(178, 199, 208);
        private readonly Vec3b WarSideLoseDefenseColor = new Vec3b( 27,  29,  33);

        public Side Scan(WarResultImage warResultImage)
        {
            if (warResultImage == null)
            {
                throw new ArgumentNullException();
            }

            var color = warResultImage.Image.At<Vec3b>(WarSidePoint.Y, WarSidePoint.X);
            var isDefense =
                ColorDiff.CompareAsBgrColor(color, WarSideWinDefenseColor,  Threshold) ||
                ColorDiff.CompareAsBgrColor(color, WarSideLoseDefenseColor, Threshold);

            return isDefense ? Side.Defence : Side.Offence;
        }
    }
}

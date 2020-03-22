using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class CountryScanner
    {
        private const int Threshold = 3;

        // 国名判定用の座標と国名・色
        private readonly Point OffenseCountryPoint = new Point(414, 90);
        private readonly Point DefenseCountryPoint = new Point(100, 90);

        private readonly Vec3b IelsordColor  = new Vec3b(255, 105,  54);
        private readonly Vec3b NetzawarColor = new Vec3b( 42,  44,  48);
        private readonly Vec3b HordaineColor = new Vec3b( 45, 222, 250);
        private readonly Vec3b GeburandColor = new Vec3b(196,  39, 161);
        private readonly Vec3b CesedriaColor = new Vec3b( 37,  231, 86);

        private Point _point;

        public CountryScanner(Side side)
        {
            _point = side == Side.Offence ? OffenseCountryPoint : DefenseCountryPoint;
        }

        public Country Scan(WarResultImage warResultImage)
        {
            if (warResultImage == null)
            {
                throw new ArgumentNullException();
            }

            var color = warResultImage.Image.At<Vec3b>(_point.Y, _point.X);

            if (ColorDiff.CompareAsBgrColor(color, IelsordColor, Threshold))
            {
                return Country.Ielsord;
            }
            else if (ColorDiff.CompareAsBgrColor(color, NetzawarColor, Threshold))
            {
                return Country.Netzawar;
            }
            else if (ColorDiff.CompareAsBgrColor(color, HordaineColor, Threshold))
            {
                return Country.Hordaine;
            }
            else if (ColorDiff.CompareAsBgrColor(color, GeburandColor, Threshold))
            {
                return Country.Geburand;
            }
            else if (ColorDiff.CompareAsBgrColor(color, CesedriaColor, Threshold))
            {
                return Country.Cesedria;
            }

            throw new Exception("unexpected image");
        }
    }
}

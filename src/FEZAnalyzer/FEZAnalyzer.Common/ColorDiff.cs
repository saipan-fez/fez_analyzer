using Colourful;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.Common
{
    public static class ColorDiff
    {
        private static CIE76ColorDifference _difference = new CIE76ColorDifference();
        private static IColorConverter<RGBColor, LabColor> _converter  = new ConverterBuilder().FromRGB().ToLab(Illuminants.D65).Build();

        public static bool CompareAsLabColor(Vec3b cmp1, Vec3b cmp2, double threashold)
        {
            var lab1 = _converter.Convert(RGBColor.FromRGB8Bit(cmp1.Item0, cmp1.Item1, cmp1.Item2));
            var lab2 = _converter.Convert(RGBColor.FromRGB8Bit(cmp2.Item0, cmp2.Item1, cmp2.Item2));
            var diff = _difference.ComputeDifference(lab1, lab2);

            return Math.Abs(diff) < threashold;
        }

        public static bool CompareAsBgrColor(Vec3b cmp1, Vec3b cmp2, int threashold)
        {
            return
                Math.Abs(cmp1.Item0 - cmp2.Item0) < threashold &&
                Math.Abs(cmp1.Item1 - cmp2.Item1) < threashold &&
                Math.Abs(cmp1.Item2 - cmp2.Item2) < threashold;
        }
    }
}

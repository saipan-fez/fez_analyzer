using OpenCvSharp;
using System;

namespace FEZAnalyzer.Entity
{
    public class WarResultImage : IDisposable
    {
        public Mat<Vec3b> Image { get; }
        public long TimeStamp { get; }

        public WarResultImage(Mat<Vec3b> bitmap) :
            this(bitmap, DateTime.Now.Ticks)
        { }

        public WarResultImage(Mat<Vec3b> image, long timeStamp)
        {
            Image     = image ?? throw new ArgumentNullException();
            TimeStamp = timeStamp;
        }

        public void Dispose()
        {
            Image?.Dispose();
        }
    }
}

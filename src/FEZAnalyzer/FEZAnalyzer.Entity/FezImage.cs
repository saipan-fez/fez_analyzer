using OpenCvSharp;
using System;

namespace FEZAnalyzer.Entity
{
    public class FezImage : IDisposable
    {
        public Mat<Vec3b> Image { get; }
        public long TimeStamp { get; }

        public FezImage(Mat<Vec3b> image) : this(image, DateTime.Now.Ticks)
        {
        }

        public FezImage(Mat<Vec3b> image, long timeStamp)
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

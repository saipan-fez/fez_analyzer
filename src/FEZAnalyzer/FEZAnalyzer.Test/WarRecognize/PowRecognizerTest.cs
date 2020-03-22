using System.Threading.Tasks;
using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class PowRecognizerTest
    {
        PowRecognizer recognizer;

        [TestInitialize]
        public async Task Initialize()
        {
            recognizer = await PowRecognizer.CreateAsync();
        }

        [TestMethod]
        public void Pow100()
        {
            using (var m = new Mat("TestImages\\pow\\100.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(100, pow);
            }
        }

        [TestMethod]
        public void Pow32()
        {
            using (var m = new Mat("TestImages\\pow\\32.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(32, pow);
            }
        }

        [TestMethod]
        public void Pow56()
        {
            using (var m = new Mat("TestImages\\pow\\56.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(56, pow);
            }
        }

        [TestMethod]
        public void Pow72()
        {
            using (var m = new Mat("TestImages\\pow\\72.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(72, pow);
            }
        }

        [TestMethod]
        public void Pow84()
        {
            using (var m = new Mat("TestImages\\pow\\84.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(84, pow);
            }
        }

        [TestMethod]
        public void Pow88()
        {
            using (var m = new Mat("TestImages\\pow\\88.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(88, pow);
            }
        }

        [TestMethod]
        public void Pow90()
        {
            using (var m = new Mat("TestImages\\pow\\90.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var pow = recognizer.Recognize(img);
                Assert.AreEqual(90, pow);
            }
        }
    }
}

using System.Threading.Tasks;
using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class MapNameRecognizerTest
    {
        MapNameRecognizer recognizer;

        [TestInitialize]
        public async Task Initialize()
        {
            recognizer = await MapNameRecognizer.CreateAsync();
        }

        [TestMethod]
        public void マップ名が取得できているか()
        {
            using (var m   = new Mat("TestImages\\Map\\MapTest.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var map = recognizer.Recognize(img);
                Assert.AreEqual(map.Name, "カペラ隕石跡");
            }
        }

        [TestMethod]
        public void 闘技場が取得できているか()
        {
            using (var m = new Mat("TestImages\\Map\\Tougijou.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var map = recognizer.Recognize(img);
                Assert.AreEqual(map.Name, "闘技場");
            }
        }
    }
}

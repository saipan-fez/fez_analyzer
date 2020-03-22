using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class CostDisplayRecognizerTest
    {
        CostDisplayRecognizer recognizer;

        [TestInitialize]
        public void Initialize()
        {
            recognizer = new CostDisplayRecognizer();
        }

        [TestMethod]
        public void コスト表示()
        {
            using (var m = new Mat("TestImages\\Cost\\CostDisplay.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                Assert.IsTrue(recognizer.Recognize(img));
            }
        }

        [TestMethod]
        public void コスト非表示()
        {
            using (var m = new Mat("TestImages\\Cost\\CostUndisplay.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                Assert.IsFalse(recognizer.Recognize(img));
            }
        }
    }
}

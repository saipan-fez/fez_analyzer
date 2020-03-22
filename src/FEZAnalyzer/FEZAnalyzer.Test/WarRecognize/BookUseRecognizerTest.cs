using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class BookUseRecognizerTest
    {
        BookUseRecognizer recognizer;

        [TestInitialize]
        public void Initialize()
        {
            recognizer = new BookUseRecognizer();
        }

        [TestMethod]
        public void 書の使用ON()
        {
            using (var m   = new Mat("TestImages\\BookUse\\BookOn.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                Assert.IsTrue(recognizer.Recognize(img));
            }
        }

        [TestMethod]
        public void 書の使用OFF()
        {
            using (var m = new Mat("TestImages\\BookUse\\BookOff.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                Assert.IsFalse(recognizer.Recognize(img));
            }
        }
    }
}

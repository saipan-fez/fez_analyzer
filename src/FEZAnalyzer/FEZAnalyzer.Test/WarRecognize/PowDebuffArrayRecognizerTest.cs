using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class PowDebuffArrayRecognizerTest
    {
        PowDebuffArrayRecognizer recognizer;

        [TestInitialize]
        public void Initialize()
        {
            recognizer = new PowDebuffArrayRecognizer();
        }

        [TestMethod]
        public void パワーブレイク無し状態を取得できるか()
        {
            using (var m = new Mat("TestImages\\PowerBreak\\nonpowerbreak.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var powDebuffs = recognizer.Recognize(img);
                Assert.IsTrue(powDebuffs.Length == 0);
            }
        }

        [TestMethod]
        public void パワーブレイク有り状態を取得できるか()
        {
            // 一番右にパワーブレイクアイコン
            using (var m = new Mat("TestImages\\PowerBreak\\powerbreak1.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var powDebuffs = recognizer.Recognize(img);
                Assert.IsTrue(powDebuffs.Length == 1);
                Assert.AreEqual(powDebuffs[0].Name, "パワーブレイク");
            }

            // 右から2番目にパワーブレイクアイコン
            using (var m = new Mat("TestImages\\PowerBreak\\powerbreak2.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var powDebuffs = recognizer.Recognize(img);
                Assert.IsTrue(powDebuffs.Length == 1);
                Assert.AreEqual(powDebuffs[0].Name, "パワーブレイク");
            }
        }
    }
}

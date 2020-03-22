using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class TotalScoreScannerTest
    {
        NewUiWarScoreClipper newUiWarScoreClipper = new NewUiWarScoreClipper();
        AttackTotalScanner attack;
        RegionTotalScanner region;
        SupportTotalScanner support;

        [TestInitialize]
        public void Initialize()
        {
            attack = new AttackTotalScanner();
            region = new RegionTotalScanner();
            support = new SupportTotalScanner();
        }

        [TestMethod]
        public void _20146()
        {
            using (var m = new Mat("TestImages\\NewUI\\TotalScore\\20146.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(20146, attack.Scan(warResultImage));
                Assert.AreEqual(26584, region.Scan(warResultImage));
                Assert.AreEqual(5304, support.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _27191()
        {
            using (var m = new Mat("TestImages\\NewUI\\TotalScore\\27191.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(27191, attack.Scan(warResultImage));
                Assert.AreEqual(0, region.Scan(warResultImage));
                Assert.AreEqual(651, support.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _33862()
        {
            using (var m = new Mat("TestImages\\NewUI\\TotalScore\\33862.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(33862, attack.Scan(warResultImage));
                Assert.AreEqual(505, region.Scan(warResultImage));
                Assert.AreEqual(1584, support.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _38076()
        {
            using (var m = new Mat("TestImages\\NewUI\\TotalScore\\38076.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(38076, attack.Scan(warResultImage));
                Assert.AreEqual(8339, region.Scan(warResultImage));
                Assert.AreEqual(10773, support.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _52413()
        {
            using (var m = new Mat("TestImages\\NewUI\\TotalScore\\52413.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(52413, attack.Scan(warResultImage));
                Assert.AreEqual(7105, region.Scan(warResultImage));
                Assert.AreEqual(13800, support.Scan(warResultImage));
            }
        }
    }
}

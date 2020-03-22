using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class CountScoreScannerTest
    {
        NewUiWarScoreClipper newUiWarScoreClipper = new NewUiWarScoreClipper();
        KillCountScanner kill;
        DeadCountScanner dead;
        BuildingCountScanner building;
        BuildingDestroyCountScanner buildingDestroy;
        CrystalCountScanner crystal;

        [TestInitialize]
        public void Initialize()
        {
            kill            = new KillCountScanner();
            dead            = new DeadCountScanner();
            building        = new BuildingCountScanner();
            buildingDestroy = new BuildingDestroyCountScanner();
            crystal         = new CrystalCountScanner();
        }

        [TestMethod]
        public void _2_3_0_0_0()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\2_3_0_0_0.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(2, kill.Scan(img));
                Assert.AreEqual(3, dead.Scan(img));
                Assert.AreEqual(0, building.Scan(img));
                Assert.AreEqual(0, buildingDestroy.Scan(img));
                Assert.AreEqual(0, crystal.Scan(img));
            }
        }

        [TestMethod]
        public void _2_4_2_0_9()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\2_4_2_0_9.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(2, kill.Scan(img));
                Assert.AreEqual(4, dead.Scan(img));
                Assert.AreEqual(2, building.Scan(img));
                Assert.AreEqual(0, buildingDestroy.Scan(img));
                Assert.AreEqual(9, crystal.Scan(img));
            }
        }

        [TestMethod]
        public void _5_4_0_1_0()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\5_4_0_1_0.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(5, kill.Scan(img));
                Assert.AreEqual(4, dead.Scan(img));
                Assert.AreEqual(0, building.Scan(img));
                Assert.AreEqual(1, buildingDestroy.Scan(img));
                Assert.AreEqual(0, crystal.Scan(img));
            }
        }

        [TestMethod]
        public void _5_4_2_0_16()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\5_4_2_0_16.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(5, kill.Scan(img));
                Assert.AreEqual(4, dead.Scan(img));
                Assert.AreEqual(2, building.Scan(img));
                Assert.AreEqual(0, buildingDestroy.Scan(img));
                Assert.AreEqual(16, crystal.Scan(img));
            }
        }

        [TestMethod]
        public void _7_3_2_0_7()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\7_3_2_0_7.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(7, kill.Scan(img));
                Assert.AreEqual(3, dead.Scan(img));
                Assert.AreEqual(2, building.Scan(img));
                Assert.AreEqual(0, buildingDestroy.Scan(img));
                Assert.AreEqual(7, crystal.Scan(img));
            }
        }

        [TestMethod]
        public void _8_5_1_0_15()
        {
            using (var m = new Mat("TestImages\\NewUI\\CountScore\\8_5_1_0_15.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var img = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(8, kill.Scan(img));
                Assert.AreEqual(5, dead.Scan(img));
                Assert.AreEqual(1, building.Scan(img));
                Assert.AreEqual(0, buildingDestroy.Scan(img));
                Assert.AreEqual(15, crystal.Scan(img));
            }
        }
    }
}
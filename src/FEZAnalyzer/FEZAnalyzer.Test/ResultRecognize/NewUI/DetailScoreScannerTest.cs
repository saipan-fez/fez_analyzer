using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class DetailScoreScannerTest
    {
        NewUiWarScoreClipper newUiWarScoreClipper = new NewUiWarScoreClipper();
        PlayerDamageDetailScanner playerDamage;
        KillDamageDetailScanner killDamasge;
        SummonReleaseDetailScanner summonRelease;
        BuildingDamageDetailScanner buildingDamage;
        RegionDestroyDetailScanner regionDestroy;
        RegionDamageDetailScanner regionDamage;
        ContributionDetailScanner contribution;
        CrystalOperationDetailScanner crystalOperation;
        SummonActionDetailScanner summonAction;

        [TestInitialize]
        public void Initialize()
        {
            playerDamage     = new PlayerDamageDetailScanner();
            killDamasge      = new KillDamageDetailScanner();
            summonRelease    = new SummonReleaseDetailScanner();
            buildingDamage   = new BuildingDamageDetailScanner();
            regionDestroy    = new RegionDestroyDetailScanner();
            regionDamage     = new RegionDamageDetailScanner();
            contribution     = new ContributionDetailScanner();
            crystalOperation = new CrystalOperationDetailScanner();
            summonAction     = new SummonActionDetailScanner();
        }

        [TestMethod]
        public void _17906()
        {
            using (var m = new Mat("TestImages\\NewUI\\DetailScore\\17906.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(17906, playerDamage.Scan(warResultImage));
                Assert.AreEqual(21410, killDamasge.Scan(warResultImage));
                Assert.AreEqual(199, summonRelease.Scan(warResultImage));
                Assert.AreEqual(5233, buildingDamage.Scan(warResultImage));
                Assert.AreEqual(2975, regionDestroy.Scan(warResultImage));
                Assert.AreEqual(4712, regionDamage.Scan(warResultImage));
                Assert.AreEqual(42, contribution.Scan(warResultImage));
                Assert.AreEqual(51, crystalOperation.Scan(warResultImage));
                Assert.AreEqual(0, summonAction.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _18436()
        {
            using (var m = new Mat("TestImages\\NewUI\\DetailScore\\18436.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(18436, playerDamage.Scan(warResultImage));
                Assert.AreEqual(29909, killDamasge.Scan(warResultImage));
                Assert.AreEqual(1182, summonRelease.Scan(warResultImage));
                Assert.AreEqual(1745, buildingDamage.Scan(warResultImage));
                Assert.AreEqual(0, regionDestroy.Scan(warResultImage));
                Assert.AreEqual(10962, regionDamage.Scan(warResultImage));
                Assert.AreEqual(55, contribution.Scan(warResultImage));
                Assert.AreEqual(57, crystalOperation.Scan(warResultImage));
                Assert.AreEqual(0, summonAction.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _19541()
        {
            using (var m = new Mat("TestImages\\NewUI\\DetailScore\\19541.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(19541, playerDamage.Scan(warResultImage));
                Assert.AreEqual(35020, killDamasge.Scan(warResultImage));
                Assert.AreEqual(410, summonRelease.Scan(warResultImage));
                Assert.AreEqual(667, buildingDamage.Scan(warResultImage));
                Assert.AreEqual(0, regionDestroy.Scan(warResultImage));
                Assert.AreEqual(10478, regionDamage.Scan(warResultImage));
                Assert.AreEqual(88, contribution.Scan(warResultImage));
                Assert.AreEqual(114, crystalOperation.Scan(warResultImage));
                Assert.AreEqual(4, summonAction.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _21648()
        {
            using (var m = new Mat("TestImages\\NewUI\\DetailScore\\21648.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(21648, playerDamage.Scan(warResultImage));
                Assert.AreEqual(22277, killDamasge.Scan(warResultImage));
                Assert.AreEqual(312, summonRelease.Scan(warResultImage));
                Assert.AreEqual(3198, buildingDamage.Scan(warResultImage));
                Assert.AreEqual(1121, regionDestroy.Scan(warResultImage));
                Assert.AreEqual(18018, regionDamage.Scan(warResultImage));
                Assert.AreEqual(62, contribution.Scan(warResultImage));
                Assert.AreEqual(78, crystalOperation.Scan(warResultImage));
                Assert.AreEqual(0, summonAction.Scan(warResultImage));
            }
        }

        [TestMethod]
        public void _8645()
        {
            using (var m = new Mat("TestImages\\NewUI\\DetailScore\\8645.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var warResultImage = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(8645, playerDamage.Scan(warResultImage));
                Assert.AreEqual(12459, killDamasge.Scan(warResultImage));
                Assert.AreEqual(0, summonRelease.Scan(warResultImage));
                Assert.AreEqual(1008, buildingDamage.Scan(warResultImage));
                Assert.AreEqual(696, regionDestroy.Scan(warResultImage));
                Assert.AreEqual(8048, regionDamage.Scan(warResultImage));
                Assert.AreEqual(35, contribution.Scan(warResultImage));
                Assert.AreEqual(27, crystalOperation.Scan(warResultImage));
                Assert.AreEqual(0, summonAction.Scan(warResultImage));
            }
        }
    }
}
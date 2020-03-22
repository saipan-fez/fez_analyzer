using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using System;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class OtherScannerTest
    {
        NewUiWarScoreClipper newUiWarScoreClipper = new NewUiWarScoreClipper();

        WarResultScanner warResultScanner      = new WarResultScanner();
        SideScanner      sideScanner           = new SideScanner();
        CountryScanner   offenceCountryScanner = new CountryScanner(Side.Offence);
        CountryScanner   defenceCountryScanner = new CountryScanner(Side.Defence);
        WarTimeScanner   warTimeScanner        = new WarTimeScanner();

        [DataTestMethod]
        [DataRow("TestImages\\NewUI\\WarResult\\Lose.png", WarResult.Lose)]
        [DataRow("TestImages\\NewUI\\WarResult\\Win.png", WarResult.Win)]
        public void 勝敗(string fileName, WarResult expected)
        {
            using (var m = new Mat(fileName))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var image = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(expected, warResultScanner.Scan(image));
            }
        }

        [DataTestMethod]
        [DataRow("TestImages\\NewUI\\Side\\LoseDeffence.png", Side.Defence)]
        [DataRow("TestImages\\NewUI\\Side\\WinDeffence.png",  Side.Defence)]
        [DataRow("TestImages\\NewUI\\Side\\LoseOffence.png",  Side.Offence)]
        [DataRow("TestImages\\NewUI\\Side\\WinOffence.png",   Side.Offence)]
        public void 攻守(string fileName, Side expected)
        {
            using (var m = new Mat(fileName))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var image = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(expected, sideScanner.Scan(image));
            }
        }

        [DataTestMethod]
        [DataRow("TestImages\\NewUI\\Country\\Cas_Nes.png", Country.Cesedria, Country.Netzawar)]
        [DataRow("TestImages\\NewUI\\Country\\Ces_Hol.png", Country.Cesedria, Country.Hordaine)]
        [DataRow("TestImages\\NewUI\\Country\\Geb_Cas.png", Country.Geburand, Country.Cesedria)]
        [DataRow("TestImages\\NewUI\\Country\\Hol_Ile.png", Country.Hordaine, Country.Ielsord)]
        [DataRow("TestImages\\NewUI\\Country\\Ile_Nes.png", Country.Ielsord,  Country.Netzawar)]
        [DataRow("TestImages\\NewUI\\Country\\Net_Geb.png", Country.Netzawar, Country.Geburand)]
        [DataRow("TestImages\\NewUI\\Country\\Net_Ile.png", Country.Netzawar, Country.Ielsord)]
        public void 国(string fileName, Country expectedDefence, Country expectedOffence)
        {
            using (var m = new Mat(fileName))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var image = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(expectedDefence, defenceCountryScanner.Scan(image));
                Assert.AreEqual(expectedOffence, offenceCountryScanner.Scan(image));
            }
        }

        [DataTestMethod]
        [DataRow("TestImages\\NewUI\\WarTime\\1551.png", "00:15:51")]
        [DataRow("TestImages\\NewUI\\WarTime\\1816.png", "00:18:16")]
        [DataRow("TestImages\\NewUI\\WarTime\\1834.png", "00:18:34")]
        [DataRow("TestImages\\NewUI\\WarTime\\1901.png", "00:19:01")]
        [DataRow("TestImages\\NewUI\\WarTime\\1931.png", "00:19:31")]
        [DataRow("TestImages\\NewUI\\WarTime\\2111.png", "00:21:11")]
        [DataRow("TestImages\\NewUI\\WarTime\\2207.png", "00:22:07")]
        public void 戦争時間(string fileName, string expectedTime)
        {
            var expectedTimeSpan = expectedTime == null ? null : (TimeSpan?)TimeSpan.Parse(expectedTime);

            using (var m = new Mat(fileName))
            using (var mat = new Mat<Vec3b>(m))
            using (var tmp = new FezImage(mat))
            using (var image = newUiWarScoreClipper.Clip(tmp))
            {
                Assert.AreEqual(expectedTimeSpan, warTimeScanner.Scan(image));
            }
        }
    }
}

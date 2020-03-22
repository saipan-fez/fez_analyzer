using System.Threading.Tasks;
using FEZAnalyzer.Entity;
using FEZAnalyzer.WarRecognize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;

namespace FEZAnalyzer.Test
{
    [TestClass]
    public class SkillCollectionRecognizerTest
    {
        SkillCollectionRecognizer recognizer;

        [TestInitialize]
        public async Task Initialize()
        {
            recognizer = await SkillCollectionRecognizer.CreateAsync();
        }

        [TestMethod]
        public void 片手()
        {
            using (var m = new Mat("TestImages\\Skill\\katate.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var skills = recognizer.Recognize(img);
                Assert.AreEqual(skills[0].Name, "シールドバッシュ");
                Assert.AreEqual(skills[1].Name, "ガードレインフォース");
                Assert.AreEqual(skills[2].Name, "ブレイズスラッシュ");
                Assert.AreEqual(skills[3].Name, "アーススタンプ");
                Assert.AreEqual(skills[4].Name, "クランブルストーム");
                Assert.AreEqual(skills[5].Name, "ソリッドウォール");
                Assert.AreEqual(skills[6].Name, "エンダーペイン");
                Assert.AreEqual(skills[7].Name, "スラムアタック");
            }
        }

        [TestMethod]
        public void 氷皿()
        {
            using (var m = new Mat("TestImages\\Skill\\koori.png"))
            using (var mat = new Mat<Vec3b>(m))
            using (var img = new FezImage(mat))
            {
                var skills = recognizer.Recognize(img);
                Assert.AreEqual(skills[0].Name, "ブリザードカレス");
                Assert.AreEqual(skills[1].Name, "アイスジャベリン");
                Assert.AreEqual(skills[2].Name, "ライトニングスピア");
                Assert.AreEqual(skills[3].Name, "フリージングウェイブ");
                Assert.AreEqual(skills[4].Name, "サンダーボルト");
                Assert.AreEqual(skills[5].Name, "ライトニング");
                Assert.AreEqual(skills[6].Name, "詠唱");
                Assert.AreEqual(skills[7].Name, "アイスボルト");
            }
        }
    }
}

using FEZAnalyzer.Setting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.Test
{
    [TestClass]
    public class SettingReaderTest
    {
        // FEZが入っていない環境では動作しない(設定値を読み取れない)ため、
        // 単体テストコードは記述しない。

        //// デバッグ用テストコード
        //[TestMethod]
        //public async Task Test()
        //{
        //    var globalSetting = await SettingReader.ReadGlobalSettingAsync();
        //}
    }
}

using FEZAnalyzer.Entity;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// フルスクリーンモードのFEZ画像撮影クラス
    /// </summary>
    internal class FullScreenModeImageCapture : ICapture
    {
        public bool TryCapture(out FezImage fezImage)
        {
            fezImage = null;
            return false;
        }
    }
}

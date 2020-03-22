using FEZAnalyzer.Entity;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// FEZ画像撮影インターフェース
    /// </summary>
    public interface ICapture
    {
        /// <summary>
        /// 撮影する
        /// </summary>
        /// <param name="fezImage">撮影した画像</param>
        /// <returns>撮影成否</returns>
        bool TryCapture(out FezImage fezImage);
    }
}

using FEZAnalyzer.Entity;

namespace FEZAnalyzer.WarRecognize
{
    /// <summary>
    /// 戦争中情報解析インターフェース
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IWarRecognize<T>
    {
        T Recognize(FezImage fezImage);
    }
}

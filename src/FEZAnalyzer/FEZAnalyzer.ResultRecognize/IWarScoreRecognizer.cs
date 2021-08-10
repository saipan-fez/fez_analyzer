using FEZAnalyzer.Entity;

namespace FEZAnalyzer.ResultRecognize
{
    public interface IWarScoreRecognizer
    {
        bool TryRecognize(FezImage fezImage, out WarScore warScore);
    }
}
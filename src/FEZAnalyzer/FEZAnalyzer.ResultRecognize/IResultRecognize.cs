using FEZAnalyzer.Entity;

namespace FEZAnalyzer.ResultRecognize
{
    internal interface IWarScoreRecognize
    {
        WarScore Recognize(FezImage fezImage);
    }
}

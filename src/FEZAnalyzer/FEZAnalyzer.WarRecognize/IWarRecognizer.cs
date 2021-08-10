using FEZAnalyzer.Entity;

namespace FEZAnalyzer.WarRecognize
{
    public interface IWarRecognizer
    {
        bool TryRecognize(FezImage fezImage, out WarInfo warInfo);
    }
}
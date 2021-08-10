using FEZAnalyzer.Entity;
using FEZAnalyzer.ResultRecognize.NewUI;

namespace FEZAnalyzer.ResultRecognize
{
    public class WarScoreRecognizer : IWarScoreRecognizer
    {
        private NewUiWarScoreRecognizer newUiWarScoreRecognizer;

        public WarScoreRecognizer()
        {
            newUiWarScoreRecognizer = new NewUiWarScoreRecognizer();
        }

        public bool TryRecognize(FezImage fezImage, out WarScore warScore)
        {
            if (newUiWarScoreRecognizer.TryRecognize(fezImage, out warScore))
            {
                return true;
            }

            return false;
        }
    }
}

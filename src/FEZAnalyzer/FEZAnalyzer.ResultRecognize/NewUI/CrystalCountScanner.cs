using FEZAnalyzer.ResultRecognize.NewUI.Base;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class CrystalCountScanner : CountScoreScanner
    {
        public CrystalCountScanner()
            : base(170, 632)
        { }
    }
}

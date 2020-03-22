using FEZAnalyzer.ResultRecognize.NewUI.Base;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class KillCountScanner : CountScoreScanner
    {
        public KillCountScanner()
            : base(170, 568)
        { }
    }
}


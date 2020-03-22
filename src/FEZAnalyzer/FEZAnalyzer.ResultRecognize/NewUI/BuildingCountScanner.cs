using FEZAnalyzer.ResultRecognize.NewUI.Base;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class BuildingCountScanner : CountScoreScanner
    {
        public BuildingCountScanner()
            : base(170, 600)
        { }
    }
}

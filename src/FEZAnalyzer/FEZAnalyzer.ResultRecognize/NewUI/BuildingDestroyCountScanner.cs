using FEZAnalyzer.ResultRecognize.NewUI.Base;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class BuildingDestroyCountScanner : CountScoreScanner
    {
        public BuildingDestroyCountScanner()
            : base(170, 616)
        { }
    }
}

﻿using FEZAnalyzer.ResultRecognize.NewUI.Base;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("FEZAnalyzer.Test")]
namespace FEZAnalyzer.ResultRecognize.NewUI
{
    internal class DeadCountScanner : CountScoreScanner
    {
        public DeadCountScanner()
            : base(170, 584)
        { }
    }
}

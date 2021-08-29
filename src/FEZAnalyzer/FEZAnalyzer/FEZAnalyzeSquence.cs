using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FEZAnalyzer.Entity;
using FEZAnalyzer.ImageCapture;
using FEZAnalyzer.ResultRecognize;
using FEZAnalyzer.SkillCount;
using FEZAnalyzer.WarRecognize;
using Microsoft.Extensions.Logging;

namespace FEZAnalyzer
{
    internal class FEZAnalyzeSquence : IDisposable
    {
        private readonly TimeSpan WaitNextFrameTimeSpan = TimeSpan.FromMilliseconds(33d);
        private readonly TimeSpan WaitProcessTimeSpan = TimeSpan.FromSeconds(1d);

        private readonly IWarRecognizer      _warRecognizer;
        private readonly IWarScoreRecognizer _warScoreRecognizer;
        private readonly ISkillUseRecognizer _skillUseRecognizer;
        private readonly CaptureProvider     _captureProvider;

        private readonly ILogger<FEZAnalyzeSquence> _logger;

        public FEZAnalyzeSquence(
            IWarRecognizer warRecognizer,
            IWarScoreRecognizer warScoreRecognizer)
        {
            _warRecognizer = warRecognizer;
            _warScoreRecognizer = warScoreRecognizer;
            _captureProvider = new CaptureProvider();
        }

        #region IDisposable 
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                Trace.TraceInformation($"{nameof(FEZAnalyzeSquence)} is Disposed.");
            }
            finally
            {
                _disposed = true;
            }
        }
        #endregion

        public async Task<TimeSpan> ProcessAsync()
        {
            // ----------------------------------
            // TODO: シーケンスの整理が必要 (戦争中にFOした場合/強制終了なども考慮する)
            // ----------------------------------

            var capture = await _captureProvider.GetCaptureAsync();
            if (capture == null)
            {
                // キャプチャ対象がない(=FEZが起動していない)場合
                // 5秒間隔でプロセスが起動しているか監視させる
                return WaitProcessTimeSpan;
            }

            if (!capture.TryCapture(out FezImage fezImage))
            {
                // キャプチャできない場合
                // 次フレームまでwaitさせる
                // (首都掲示板を開いて変な画像がキャプチャされた場合などが該当)
                return WaitNextFrameTimeSpan;
            }

            if (_warRecognizer.TryRecognize(fezImage, out WarInfo warInfo))
            {
                var usedSkill = _skillUseRecognizer.RecognizeUsedSkill(
                    fezImage.TimeStamp,
                    warInfo.Pow,
                    warInfo.Skills,
                    warInfo.PowDebuffs);

                // TODO: スキルの使用解析
                return WaitNextFrameTimeSpan;
            }

            if (_warScoreRecognizer.TryRecognize(fezImage, out WarScore warScore))
            {
                // 戦争が終了
                return WaitNextFrameTimeSpan;
            }

            // 戦争中の暗転時(死亡時)など戦争中/戦争結果ともに解析できなかった場合
            // 次のフレームまでwaitさせる
            return WaitNextFrameTimeSpan;
        }
    }
}

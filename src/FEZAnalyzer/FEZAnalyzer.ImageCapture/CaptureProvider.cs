using FEZAnalyzer.Setting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// FEZ画像提供クラス
    /// </summary>
    public class CaptureProvider : IDisposable
    {
        private int?  _lastProcessId        = null;
        private long? _lastProcessStartTime = null;

        private ICapture _capture = null;

        public CaptureProvider()
        {
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
                Trace.TraceInformation($"{nameof(CaptureProvider)} is Disposed.");
            }
            finally
            {
                _disposed = true;
            }
        }
        #endregion

        public async Task<ICapture> GetCaptureAsync()
        {
            Process[] processes = null;

            try
            {
                var processList = Process.GetProcessesByName("FEzero_Client");
                var p = processList.FirstOrDefault();
                if (p == null)
                {
                    return null;
                }

                var processId        = p.Id;
                var processStartTime = p.StartTime.Ticks;

                // プロセスが同一の場合は前回のキャプチャ提供オブジェクトを返す
                if (processId        == _lastProcessId &&
                    processStartTime == _lastProcessStartTime &&
                    _capture         != null)
                {
                    p.Dispose();
                    return _capture;
                }

                // 次回呼び出し時に同じプロセスで動き続けていた場合は、
                // キャプチャ提供オブジェクトを使いまわせるようプロセスIDとプロセス開始時間をキャッシュ
                _lastProcessId        = processId;
                _lastProcessStartTime = processStartTime;

                var setting = await SettingReader.ReadGlobalSettingAsync();

                // TODO: Fullscreenの値が 0 or 1 以外の場合の挙動確認
                // TODO: GLOBAL.INIがない場合のデフォルト設定の挙動確認

                // ウィンドウとフルスクリーンでキャプチャ方式が異なるため、
                // フルスクリーン設定から生成するICaptureオブジェクトを振り分ける
                var isWindowMode  = setting.Fullscreen == 0;
                if (isWindowMode)
                {
                    _capture = new WindowModeImageCapture(p.Id);
                }
                else
                {
                    // TODO: フルスクリーン時のキャプチャ提供クラスの実装
                    throw new NotImplementedException("full screen mode capture is not developed.");
                    //_capture = new FullScreenModeImageCapture();
                }
            }
            finally
            {
                if (processes != null)
                {
                    foreach (var p in processes)
                    {
                        p.Dispose();
                    }
                }
            }

            return _capture;
        }
    }
}

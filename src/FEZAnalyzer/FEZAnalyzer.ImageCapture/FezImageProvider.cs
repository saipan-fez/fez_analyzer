using FEZAnalyzer.Entity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// FEZ画像提供クラス
    /// </summary>
    public class FezImageProvider : IDisposable
    {
        private bool _disposed = false;
        private CancellationTokenSource _cts  = null;
        private Task                    _task = null;

        private ICapture _capture = null;

        /// <summary>
        /// 提供可能かどうか
        /// </summary>
        public bool IsProvidable { get; private set; }

        /// <summary>
        /// FEZ画像
        /// </summary>
        public FezImage Image { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="capture"></param>
        public FezImageProvider(ICapture capture)
        {
            if (capture == null)
            {
                throw new ArgumentNullException();
            }

            _capture = capture;
        }

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
                Stop();

                if (_cts != null)
                {
                    _cts.Dispose();
                    _cts = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        public void Start()
        {
            if (_cts != null)
            {
                return;
            }

            try
            {
                _cts      = new CancellationTokenSource();
                var token = _cts.Token;
                _task     = Task.Run(() => { UpdateImage(token); }, token);
            }
            catch
            {
                Stop();
                throw;
            }
        }

        public void Stop()
        {
            if (_cts == null)
            {
                return;
            }

            try
            {
                _cts.Cancel(false);
                _task.Wait();
            }
            catch
            {
                throw;
            }
            finally
            {
                _cts.Dispose();
                _cts  = null;
                _task = null;
            }
        }

        private void UpdateImage(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var result = _capture.TryCapture(out FezImage fezImage);

                    /// TODO: 画像Disposeを考慮(別スレッドとの兼ね合い)
                    IsProvidable = result;
                    Image        = fezImage;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}

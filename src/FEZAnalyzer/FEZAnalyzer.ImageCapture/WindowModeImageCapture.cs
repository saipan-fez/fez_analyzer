using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using NativeMethods = FEZAnalyzer.Common.NativeMethods;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// ウィンドウモードのFEZ画像撮影クラス
    /// </summary>
    internal class WindowModeImageCapture : ICapture, IDisposable
    {
        private readonly Process _process;

        public WindowModeImageCapture(int processId)
        {
            _process = Process.GetProcessById(processId);
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
                if (_process != null)
                {
                    _process.Dispose();
                }

                Trace.TraceInformation($"{nameof(WindowModeImageCapture)} is Disposed.");
            }
            finally
            {
                _disposed = true;
            }
        }
        #endregion

        public bool TryCapture(out FezImage fezImage)
        {
            fezImage = null;

            if (_process.HasExited)
            {
                return false;
            }

            var handle = _process.MainWindowHandle;
            if (handle == IntPtr.Zero ||
                !NativeMethods.GetWindowRect(handle, out RECT rect))
            {
                return false;
            }

            // 解像度が1024x768未満の場合は不正なキャプチャ
            // (掲示板を開いた時、FEZ終了時などが該当)
            var width  = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            if (width < 1024 || height < 768)
            {
                return false;
            }

            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (var graphics = Graphics.FromImage(bmp))
                {
                    var hdc = NativeMethods.GetDC(handle);

                    NativeMethods.BitBlt(graphics.GetHdc(), 0, 0, width, height, hdc, 0, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);

                    NativeMethods.ReleaseDC(handle, hdc);
                }

                using (var mat = BitmapConverter.ToMat(bmp))
                {
                    fezImage = new FezImage(new Mat<Vec3b>(mat), DateTime.Now.Ticks);
                }

                return true;
            }
        }
    }
}

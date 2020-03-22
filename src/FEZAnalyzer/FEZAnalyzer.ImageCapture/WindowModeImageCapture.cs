using FEZAnalyzer.Common;
using FEZAnalyzer.Entity;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NativeMethods = FEZAnalyzer.Common.NativeMethods;
using Size = System.Drawing.Size;

namespace FEZAnalyzer.ImageCapture
{
    /// <summary>
    /// ウィンドウモードのFEZ画像撮影クラス
    /// </summary>
    public class WindowModeImageCapture : ICapture
    {
        public bool TryCapture(out FezImage fezImage)
        {
            // TODO: テスト
            var timestamp = DateTime.Now.Ticks;

            using (var process = Process.GetProcessesByName("FEzero_Client").FirstOrDefault())
            {
                fezImage = null;

                if (process == null)
                {
                    return false;
                }

                if (!NativeMethods.GetWindowRect(process.MainWindowHandle, out RECT rect) ||
                    process.MainWindowHandle == IntPtr.Zero)
                {
                    return false;
                }

                // 解像度が1024x768以下の場合は不正なキャプチャ
                // (掲示板を開いた時、FEZ終了時などが該当)
                var size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
                if (size.Width < 1024 || size.Height < 768)
                {
                    return false;
                }

                var bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
                using (var graphics = Graphics.FromImage(bmp))
                {
                    var hdc = NativeMethods.GetDC(process.MainWindowHandle);

                    NativeMethods.BitBlt(graphics.GetHdc(), 0, 0, size.Width, size.Height, hdc, 0, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);

                    NativeMethods.ReleaseDC(process.MainWindowHandle, hdc);
                }

                using (var mat = BitmapConverter.ToMat(bmp))
                {
                    fezImage = new FezImage(new Mat<Vec3b>(mat), timestamp);
                }

                return true;
            }
        }
    }
}

﻿using System;
using System.Runtime.InteropServices;

namespace FEZAnalyzer.Common
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    internal static class NativeMethods
    {
        public enum TernaryRasterOperations : uint
        {
          SRCCOPY     = 0x00CC0020,
          SRCPAINT    = 0x00EE0086,
          SRCAND      = 0x008800C6,
          SRCINVERT   = 0x00660046,
          SRCERASE    = 0x00440328,
          NOTSRCCOPY  = 0x00330008,
          NOTSRCERASE = 0x001100A6,
          MERGECOPY   = 0x00C000CA,
          MERGEPAINT  = 0x00BB0226,
          PATCOPY     = 0x00F00021,
          PATPAINT    = 0x00FB0A09,
          PATINVERT   = 0x005A0049,
          DSTINVERT   = 0x00550009,
          BLACKNESS   = 0x00000042,
          WHITENESS   = 0x00FF0062,
          CAPTUREBLT  = 0x40000000
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("Kernel32.dll", EntryPoint ="RtlMoveMemory")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
    }
}

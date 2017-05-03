using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Servicios
{
    /// <summary>
    /// Provides functions to capture the entire screen, or a particular window, and save it to a file.
    /// </summary>
    public class FrameProvider
    {
        private readonly IntPtr hwnd;//regnum.exe
        private static FrameProvider _frameProvider;

        private FrameProvider(string processName)
        {
            var proc = Process.GetProcessesByName(processName)[0];
            hwnd = proc.MainWindowHandle;
        }

        public static FrameProvider GetFrameProvider(string processName)
        {
            return _frameProvider ?? (_frameProvider = new FrameProvider(processName));
        }

        public Bitmap GetPartial(int x, int y, int cropWidth, int cropHeight)
        {
            var partial = PrintWindow(x, y, cropWidth, cropHeight);
            return partial;
        }

        public Bitmap PrintWindow()
        {
            return PrintWindow(32, 32, 1600, 900);
        }

        private Bitmap PrintWindow(int x, int y, int cropWidth, int cropHeight)
        {
            Bitmap src = new Bitmap(2000, 1000, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(src);

            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);

            var retorno = CropBitmap(src, x, y, cropWidth, cropHeight);
            src.Dispose();
            gfxBmp.Dispose();
            return retorno;
        }

        private Bitmap CropBitmap(Bitmap bitmap, int x, int y, int cropWidth, int cropHeight)
        {
            Rectangle rect = new Rectangle(x, y, cropWidth, cropHeight);
            return new Bitmap(bitmap.Clone(rect, bitmap.PixelFormat));
        }

        public Bitmap ScaleByPercent(Bitmap imgPhoto, int percent)
        {
            float nPercent = ((float)percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var bmPhoto = new Bitmap(destWidth, destHeight,
                PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(0, 0, destWidth, destHeight),
                new System.Drawing.Rectangle(0, 0, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            bmPhoto.Save(@"D:\Scale.png", System.Drawing.Imaging.ImageFormat.Png);
            grPhoto.Dispose();
            return bmPhoto;
        }
        
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }
}
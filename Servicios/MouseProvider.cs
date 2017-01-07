using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Servicios
{
    public class MouseProvider
    {
        private readonly IntPtr hwnd;//regnum.exe
        private static MouseProvider _mouseProvider;

        private MouseProvider(string processName)
        {
            var proc = Process.GetProcessesByName(processName)[0];
            hwnd = proc.MainWindowHandle;
        }

        public static MouseProvider GetMouseProvider(string processName)
        {
            return _mouseProvider ?? (_mouseProvider = new MouseProvider(processName));
        }

        public void PosicionarMouse(int x, int y)
        {
            Win32.POINT p = new Win32.POINT {x = x * 72 / 90, y = y * 72 / 96 };
            Win32.ClientToScreen(hwnd, ref p);
            Win32.SetCursorPos((p.x ), (p.y ));
        }

    }

    public class Win32
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
    }
}

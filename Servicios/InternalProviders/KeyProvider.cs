using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Servicios.InternalProviders
{
    /// <summary>
    /// Provides functions to capture the entire screen, or a particular window, and save it to a file.
    /// </summary>
    public class KeyProvider
    {
        private readonly IntPtr hwnd;//regnum.exe
        
        public KeyProvider(string processName)
        {
            var proc = Process.GetProcessesByName(processName)[0];
            hwnd = proc.MainWindowHandle;
        }

        public void KeyPress(string keys)
        {
            SetForegroundWindow(hwnd);
            SendKeys.SendWait(keys);
        }

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr point);
    }
}
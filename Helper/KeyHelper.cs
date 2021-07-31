using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyHelper
{
    public static class KeyExtension
    {
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const uint WM_KEYDOWN = 0x100;
        const uint WM_KEYUP = 0x101;

        public static void pressKey(Process proc, int keybind)
        {
            PostMessage(proc.MainWindowHandle, WM_KEYDOWN, keybind, 0);
            PostMessage(proc.MainWindowHandle, WM_KEYUP, keybind, 0);
        }
    }
}
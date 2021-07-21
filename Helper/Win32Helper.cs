using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Helper
{
    /// <summary>
    /// win32 函数
    /// </summary>
    class Win32Helper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int SetForegroundWindow(System.IntPtr hwnd);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundFix
{
    internal class NativeMethod
    {
        /*
         * Mechanism to rum only one instance of program by:
         * Nullfx
         * http://sanity-free.org/143/csharp_dotnet_single_instance_application.html
         * Another parts are scattered through Program.cs and Form.sc
         */
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOW_Yourself = RegisterWindowMessage("WM_ShowYourself");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}

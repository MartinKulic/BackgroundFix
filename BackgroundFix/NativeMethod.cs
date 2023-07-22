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
         * Mechanism to rum only one instance od program by:
         * https://www.codeproject.com/Articles/14353/Creating-a-Basic-Windows-Service-in-C
         * Another parts are scattered through Program.cs and Form.sc
         */
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_SHOW_Yourself = RegisterWindowMessage("WM_ShowYourself");
        [DllImport("user32")]
        //public static extern bool PostMessage(IntPtr hwnd, int msg); //works but idn what I am doing, may cause problem in future, rather not use
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}

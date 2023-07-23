using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundFix
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Mutex mutex = new Mutex(true, "DisplayFix_Mutex");
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());

                mutex.ReleaseMutex();
            } else
            {
                //NativeMethod.PostMessage((IntPtr)NativeMethod.HWND_BROADCAST, NativeMethod.WM_SHOW_Yourself);
                NativeMethod.PostMessage((IntPtr)NativeMethod.HWND_BROADCAST, NativeMethod.WM_SHOW_Yourself, IntPtr.Zero, IntPtr.Zero);
            }

        }
    }
}

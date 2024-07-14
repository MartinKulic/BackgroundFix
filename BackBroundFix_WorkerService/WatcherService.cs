using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BackBroundFix_WorkerService
{
    public partial class WatcherService
    {
        const int SM_CMONITORS = 80;

        [LibraryImport("User32.dll", EntryPoint = "GetSystemMetrics")]
        internal static partial int fGetSystemMetrics(int metric);


        public string getMessage()
        {
            int displayCount = fGetSystemMetrics(SM_CMONITORS);
            return $"Time: {DateTimeOffset.Now}{Environment.NewLine}Number of monitors: {displayCount}";
        }


    }
}

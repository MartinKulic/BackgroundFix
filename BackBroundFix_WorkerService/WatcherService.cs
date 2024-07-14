using System;
using System.Management;
using System.Runtime.InteropServices;






namespace BackBroundFix_WorkerService
{
    public class WatcherService
    {

        public string getMessage()
        {
           
            return $"Time: {DateTimeOffset.Now}{Environment.NewLine}Number of monitors: {getMonitorCount()}";
        }

        private int getMonitorCount()
        {
            int monitorCount = -1;

            try
            {
                ManagementScope scope = new("\\\\.\\ROOT\\cimv2");
                scope.Connect();
                ObjectQuery query = new("SELECT * FROM Win32_PnPEntity WHERE Service = 'monitor'");

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection result = searcher.Get();

                monitorCount = result.Count;
            }
            catch (Exception ex)
            {
                //ignore
            }

            return monitorCount;
        }

    }
}

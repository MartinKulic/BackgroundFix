using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;




namespace BackBroundFix_WorkerService
{
    public class WatcherService
    {
        public event EventHandler<int> DisplayCountChanged;
        
        private ManagementEventWatcher watcher;
       
        public int displayCount { get; set; }

        public WatcherService()
        {
            displayCount = getMonitorCount();

            try
            {
                string queryString = "SELECT * FROM __InstanceOperationEvent WITHIN 5 WHERE TargetInstance ISA 'Win32_PnPEntity' AND TargetInstance.Service = 'monitor'";
                WqlEventQuery query = new WqlEventQuery(queryString);

                watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += new EventArrivedEventHandler(OnDisplayChange);
                watcher.Start();

                DisplayCountChanged?.Invoke(this, displayCount);
            }
            catch (Exception ex)
            {
                //ignore
            }
        }



        public string getMessage()
        {
           displayCount = getMonitorCount();
            return $"Time: {DateTimeOffset.Now}{Environment.NewLine}Number of monitors: {displayCount}";
        }

        private int getMonitorCount()
        {
            int monitorCount = -1;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"\\.\ROOT\cimv2", "SELECT * FROM Win32_PnPEntity WHERE Service = 'monitor'");
                ManagementObjectCollection result = searcher.Get();

                monitorCount = result.Count;
            }
            catch (Exception ex)
            {
                //ignore
                return monitorCount;
            }

            return monitorCount;
        }

        private void OnDisplayChange(object sender, EventArrivedEventArgs e)
        {
            int newDisplaycount = getMonitorCount();
            if (newDisplaycount != displayCount)
            {
                displayCount = newDisplaycount;
                DisplayCountChanged?.Invoke(this, displayCount);
            }
        }
        private void OnDisplayCountChanged(int newDisplayCount)
        {
            DisplayCountChanged?.Invoke(this, newDisplayCount);
        }

        public void Dispose()
        {

        }
    }


}

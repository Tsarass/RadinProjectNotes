using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NotesBackupService
{
    public class DiskMonitor
    {
        private PerformanceCounter diskTimeCounter;

        public DiskMonitor()
        {
            diskTimeCounter = new PerformanceCounter("LogicalDisk","% Disk Time", "C:");
        }

        public float GetDiskTime()
        {
            diskTimeCounter.NextValue();
            Thread.Sleep(1000);
            return diskTimeCounter.NextValue();
        }
    }
}

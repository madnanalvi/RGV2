using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    internal class ProcessIOTraceEvent
    {
        public string ProcessID;
        public string ThreadID;
        public string Milliseconds;
        public string EventName;
        public string ParentProcessID;
        public string ParentThreadID;
        public string ProcessName;
        public string ThreadName;
        public int ProcessIDCount;
        public int ParentProcessIDCount;
        public int ParentThreadIDCount;
        public string Remarks;

        public ProcessIOTraceEvent(string eventName, string MillisecondsDiff, string ProcessID, string ThreadID, string ParentProcessID, string ParentThreadID, string processName, string threadName, int ProcessIDCount, int ParentProcessIDCount, int ParentThreadIDCount)
        {
            this.ProcessID = ProcessID;
            this.ThreadID = ThreadID;
            this.Milliseconds = MillisecondsDiff;
            this.EventName = eventName;
            this.ParentProcessID = ParentProcessID;
            this.ParentThreadID = ParentThreadID;
            this.ProcessName = processName;
            this.ProcessName = processName;
            this.ThreadName = threadName;
            this.ProcessIDCount = ProcessIDCount;
            this.ParentProcessIDCount = ParentProcessIDCount;
            this.ParentThreadIDCount = ParentThreadIDCount;
        }
        public ProcessIOTraceEvent(string eventName, string MillisecondsDiff, string ProcessID, string ThreadID, string ParentProcessID, string ParentThreadID, string processName, string threadName)
        {
            this.ProcessID = ProcessID;
            this.ThreadID = ThreadID;
            this.Milliseconds = MillisecondsDiff;
            this.EventName = eventName;
            this.ParentProcessID = ParentProcessID;
            this.ParentThreadID = ParentThreadID;
            this.ProcessName = processName;
            this.ProcessName = processName;
            this.ThreadName = threadName;
        }

        public override string ToString()
        {
            string str =
                " EventName=" + EventName +
                " | MillisecondsDiff=" + Milliseconds +
                " | ProcessIDCount=" + ProcessIDCount +
                " | ParentProcessIDCount=" + ParentProcessIDCount +
                " | ProcessName=" + ProcessName +
                " | ProcessID=" + ProcessID +
                " | ParentProcessID=" + ParentProcessID +
                " | ThreadName=" + ThreadName +
                " | ThreadID=" + ThreadID +
                " | ParentThreadID=" + ParentThreadID +
                " | ParentThreadIDCount=" + ParentThreadIDCount;
            return str;
        }
    }
}

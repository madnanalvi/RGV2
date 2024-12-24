using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    internal class FileIOTraceEvent
    {
        public string ID;
        public DateTime TimeStamp;
        public string Milliseconds;
        public string EventName;
        public string ProcessID;
        public string ProcessName;
        public string FilePath;
        public string NewFileName;
        public ulong FileKey;
        public ulong FileObject;
        public string CreateDisposition;
        public string CreateOptions;
        public string Remarks;

       

        public FileIOTraceEvent(string Remarks, string id, string MillisecondsDiff, string eventName, string processID, string processName, string filePath, ulong fileKey, ulong fileObject, string CreateDisposition, string CreateOptions)
        {
            this.ID = id;
            this.TimeStamp = DateTime.Now;
            this.Milliseconds = MillisecondsDiff;
            this.EventName = eventName;
            this.ProcessID = processID;
            this.ProcessName = processName;
            this.FilePath = filePath;
            this.FileKey = fileKey;
            this.FileObject = fileObject;
            this.CreateDisposition = CreateDisposition;
            this.CreateOptions = CreateOptions;
            this.Remarks = Remarks;
        }

        public override string ToString()
        {
            return String.Concat(Remarks, " - EventName=", EventName, " | ProcessName=", ProcessName, " | MillisecondsDiff=", Milliseconds, " | ProcessID=", ProcessID, " | Path=", FilePath, " | NewName=", NewFileName, " | FileKey=", FileKey, " | FileObject=", FileObject, " | CreateDisposition=", CreateDisposition, " | CreateOptions=", CreateOptions);
        }
    }
}

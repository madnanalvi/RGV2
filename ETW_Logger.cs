using System.Data;
using System.Text;

namespace MyFileIOMonitor
{
    public class ETW_Logger
    {
        //public StringBuilder cachedEvents ;
        public cacheEvents cachedEvents;
        public StringBuilder fileLog;
        StringBuilder processLog ;
        StringBuilder threadLog;
        //StringBuilder regLog ;
        //StringBuilder networkLog ;
        //StringBuilder eventLog;
        //StringBuilder allLog ;
        public DataTable fileIOLog;
        public DataTable maliciousLogs;
        public List<string> maliciousProcessNames;
        public Dictionary<int, string> ProcessList;
        public Dictionary<int, int> ParentProcessIDMapping;
        public Dictionary<int, int> ParentThreadIDMapping;
        public Dictionary<int, string> ProcessImageFileName;
        public Dictionary<string, string> FileList;
        public int infectedfilescounter = 1;
        public List<string> infectedfilenames;
        public ETW_Logger() 
        {
            cachedEvents = new cacheEvents();
            cachedEvents.AppendLine(FileIOLogFile_Headers());
            //eventLog = new StringBuilder();
            //eventLog.AppendLine("TimeStamp" + "," + "ProcessID" + "," + "ThreadID" + "," + "EventName");
            fileLog = new StringBuilder();
            fileLog.AppendLine(FileIOLogFile_Headers());
            processLog = new StringBuilder();
            processLog.AppendLine(ProcessLogFile_Headers());
            threadLog = new StringBuilder();
            threadLog.AppendLine(ThreadLogFile_Headers());
            //regLog = new StringBuilder();
            //regLog.AppendLine(RegistryLogFile_Headers());
            //networkLog = new StringBuilder();
            //networkLog.AppendLine(NetworkLogFile_Headers());
            //allLog = new StringBuilder();
            ProcessList = new Dictionary<int, string>();
            ParentProcessIDMapping = new Dictionary<int, int>();
            ParentThreadIDMapping = new Dictionary<int, int>();
            ProcessImageFileName = new Dictionary<int, string>();
            FileList = new Dictionary<string, string>();
            initDataTable();
            maliciousLogs = fileIOLog.Clone();
            maliciousLogs.Columns.Add("score", typeof(Int32));
            maliciousProcessNames = new List<string>();
            infectedfilenames = new List<string>();
        }

        private void initDataTable()
        {
            fileIOLog = new DataTable("fileIOLog");
            fileIOLog.Columns.Add("ts", typeof(Int64));
            fileIOLog.Columns.Add("pn", typeof(string));
            fileIOLog.Columns.Add("FileName", typeof(string));
            fileIOLog.Columns.Add("fn", typeof(string));            
            fileIOLog.Columns.Add("eid", typeof(string));
            fileIOLog.Columns.Add("pid", typeof(string));
            fileIOLog.Columns.Add("tid", typeof(string));            
            fileIOLog.Columns.Add("ft", typeof(string));
            fileIOLog.Columns.Add("nft", typeof(string));
            fileIOLog.Columns.Add("entropy", typeof(double));
        }

        public void AppendConsole1(RichTextBox rtb1, string logtype, string value, Color color)
        {
            //if (!ETW_Global.isInferenceMode)
            //{
                addLog(logtype, value); 
            //}
            if (ETW_Global.VERBOSE1)
            {
                if (rtb1.InvokeRequired)
                {                    
                    rtb1.Invoke(new Action<RichTextBox, string, string, Color>(AppendConsole1), new object[] { rtb1, logtype, value, color });
                    return;
                }
                rtb1.AppendText(Environment.NewLine);
                rtb1.SelectionStart = rtb1.TextLength;
                rtb1.SelectionLength = 0;
                rtb1.SelectionColor = color;
                rtb1.AppendText(value);
                rtb1.ScrollToCaret();
            }
        }

        public void addLog(string logfile, string value)
        {
            if (logfile == "file") { fileLog.AppendLine(value); }
            //else if (logfile == "network") { networkLog.AppendLine(value); }
            else if (logfile == "process") 
            { 
                processLog.AppendLine(value); 
            }
            //else if (logfile == "reg") { regLog.AppendLine(value); }
            else if (logfile == "thread") { threadLog.AppendLine(value); }
            //else if (logfile == "event") { eventLog.AppendLine(value); }
            //else { allLog.AppendLine(value); }
        }

        public void AppendConsole1(RichTextBox rtb1, string value, Color color)
        {
            if (ETW_Global.VERBOSE1)
            {
                if (rtb1.InvokeRequired)
                {
                    rtb1.Invoke(new Action<RichTextBox, string, Color>(AppendConsole1), new object[] { rtb1, value, color });
                    return;
                }
                rtb1.AppendText(Environment.NewLine);
                rtb1.SelectionStart = rtb1.TextLength;
                rtb1.SelectionLength = 0;
                rtb1.SelectionColor = color;
                rtb1.AppendText(value);
                rtb1.ScrollToCaret();
                rtb1.SelectionStart = rtb1.TextLength;
                rtb1.SelectionLength = 0;
                rtb1.SelectionColor = Color.White;
            }
        }

        public void AppendConsole2(RichTextBox rtb2, string value,Color color)
        {
            if (ETW_Global.VERBOSE2)
            {
                if (rtb2.InvokeRequired)
                {
                    rtb2.Invoke(new Action<RichTextBox, string, Color>(AppendConsole2), new object[] { rtb2, value, color });
                    return;
                }
                rtb2.AppendText(Environment.NewLine);
                rtb2.SelectionStart = rtb2.TextLength;
                rtb2.SelectionLength = 0;
                rtb2.SelectionColor = color;
                rtb2.AppendText(value);
                rtb2.ScrollToCaret();
                rtb2.SelectionStart = rtb2.TextLength;
                rtb2.SelectionLength = 0;
                rtb2.SelectionColor = Color.White;
            }
        }

        public void AppendConsole2(RichTextBox rtb2, string value)
        {
            if (ETW_Global.VERBOSE2)
            {
                if (rtb2.InvokeRequired)
                {
                    rtb2.Invoke(new Action<RichTextBox, string>(AppendConsole2), new object[] { rtb2, value });
                    return;
                }
                rtb2.AppendText(Environment.NewLine);
                rtb2.AppendText(value);
                rtb2.ScrollToCaret();
            }
        }
        public void savelogFiles()
        {
            System.IO.Directory.CreateDirectory(ETW_Global.baseDir + "log");
            //if (eventLog.Length > 1)
            //{
            //    System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\events.csv", eventLog.ToString());
            //}
            if (fileLog.Length > 1)
            {
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\FileIOlog.csv", fileLog.ToString());
            }
            //if (networkLog.Length > 1)
            //{
            //    System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Networklog.csv", networkLog.ToString());
            //}
            //if (regLog.Length > 1)
            //{
            //    System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Registorylog.csv", regLog.ToString());
            //}
            if (threadLog.Length > 1)
            {
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Threadlog.csv", threadLog.ToString());
            }
            if (processLog.Length > 1)
            {
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Processlog.csv", processLog.ToString());
            }
            //if (allLog.Length > 1)
            //{
            //    System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\log.csv", allLog.ToString());
            //}
            if (ProcessList.Count > 1)
            {
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\ProcessID_Name.csv", "ProcessID,ParentProcessID,ProcessName,ProcessImageFileName" + "\n");
                foreach (var obj in ParentProcessIDMapping)
                {
                    string processName="", pimage = "";
                    if (ProcessList.ContainsKey(obj.Key)) processName = ProcessList[obj.Key].ToString();
                    if (ProcessImageFileName.ContainsKey(obj.Key)) pimage = ProcessImageFileName[obj.Key].ToString();
                    System.IO.File.AppendAllText(ETW_Global.baseDir + "log\\ProcessID_Name.csv", obj.Key.ToString() + "," + obj.Value + "," + processName + "," + pimage + "\n");
                }
            }
            if (FileList.Count > 1)
            {
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\FileKey_Name.csv", "FileKey,FileName" + "\n");
                foreach (var obj in FileList)
                {
                    System.IO.File.AppendAllText(ETW_Global.baseDir + "log\\FileKey_Name.csv", obj.Key.ToString() + "," + obj.Value+"\n");
                }
            }
        }
        public string FileIOLogFile_Headers()
        {
            string csvstr =
                "TimeStamp" + "," +
                "TimeStampRelativeMSec" + "," +
                "ProcessID" + "," +
                "ParentProcessID" + "," +
                "ProcessName" + "," +
                "ThreadID" + "," +
                "ParentThreadID" + "," +
                "EventName" + "," +
                "EventID" + "," +
                "FilePath" + "," +
                "FileName" + "," +
                "FileType" + "," +
                "IrpPtr" + "," +
                "FileObject" + "," +
                "FileKey" + "," +
                "ExtraInfo" + "," +
                "InfoClass";
            return csvstr;
        }
        public string NetworkLogFile_Headers()
        {
            string csvstr =
                "TimeStamp" + "," +
                "TimeStampRelativeMSec" + "," +
                "ProcessID" + "," +
                "ProcessName" + "," +
                "ThreadID" + "," +
                "EventName" + "," +
                "EventID" + "," +
                "size" + "," +
                "daddr" + "," +
                "saddr" + "," +
                "dport" + "," +
                "sport" + "," +
                "mss" + "," +
                "sackopt" + "," +
                "tsopt" + "," +
                "wsopt" + "," +
                "rcvwin" + "," +
                "rcvwinscale" + "," +
                "sndwinscale" + "," +
                "eqnum" + "," +
                "connid";
            return csvstr;
        }
        public string ThreadLogFile_Headers()
        {
            string csvstr =
                "TimeStamp" + "," +
                "TimeStampRelativeMSec" + "," +
                "ProcessID" + "," +
                "ProcessName" + "," +
                "ThreadID" + "," +
                "EventName" + "," +
                "EventID" + "," +
                "ThreadName" + "," +
                "StackBase" + "," +
                "StackLimit" + "," +
                "UserStackBase" + "," +
                "UserStackLimit" + "," +
                "StartAddr" + "," +
                "Win32StartAddr" + "," +
                "TebBase" + "," +
                "SubProcessTag" + "," +
                "ParentProcessID" + "," +
                "ParentThreadID";
            return csvstr;
        }
        public string RegistryLogFile_Headers()
        {
            string csvstr =
                "TimeStamp" + "," +
                "TimeStampRelativeMSec" + "," +
                "ProcessID" + "," +
                "ProcessName" + "," +
                "ThreadID" + "," +
                "EventName" + "," +
                "EventID" + "," +
                "Status" + "," +
                "KeyHandle" + "," +
                "ElapsedTimeMSec" + "," +
                "KeyName" + "," +
                "Index";
            return csvstr;
        }
        public string ProcessLogFile_Headers()
        {
            string csvstr =
                "TimeStamp" + "," +
                "TimeStampRelativeMSec" + "," +
                "ProcessID" + "," +
                "ProcessName" + "," +
                "ThreadID" + "," +
                "EventName" + "," +
                "EventID" + "," +
                "ParentID" + "," +
                "ImageFileName" + "," +
                "DirectoryTableBase" + "," +
                "Flags" + "," +
                "SessionID" + "," +
                "ExitStatus" + "," +
                "UniqueProcessKey" + "," +
                "CommandLine";
            return csvstr;
        }
    }
}

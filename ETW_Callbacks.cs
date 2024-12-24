using Apache.Arrow;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.AutomatedAnalysis;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.StackSources;
using Microsoft.VisualBasic.Logging;
using PEFile;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace MyFileIOMonitor
{
    public class ETW_Callbacks
    {
        private List<FileIOTraceEvent> lstEvents ;
        private List<ProcessIOTraceEvent> PlstEvents ;
        ETW_Logger etwlog;
        RichTextBox _rtb1, _rtb2;        
        public ETW_Callbacks(RichTextBox rtb1, RichTextBox rtb2, ETW_Logger _etwlog) 
        {
            _rtb1 = rtb1;
            _rtb2 = rtb2;
            etwlog = _etwlog;
            lstEvents = new List<FileIOTraceEvent>();
            PlstEvents = new List<ProcessIOTraceEvent>();
        }


        #region Registry
        public void monitor_reg(RegistryTraceData obj)
        {
            //if (obj.ToString().Contains(monitored_Dir))
            //{
            //FileIOTraceEvent log = new FileIOTraceEvent("ProcessStop", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");
            //ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), "", "", obj.ProcessName, "");
            etwlog.AppendConsole1(_rtb1, "reg", RegistryLog_ToString(obj), Color.RoyalBlue);
            if (etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName))
                etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            //}
        }
        public string RegistryLog_ToString(RegistryTraceData obj)
        {
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," +
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," +
                obj.ProcessName + "," +
                obj.ThreadID + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                obj.Status + "," +
                obj.KeyHandle.ToString() + "," +
                obj.ElapsedTimeMSec + "," +
                obj.KeyName + "," +
                obj.Index;
            return csvstr;
        }

        #endregion Registry

        #region Network
        public void GetNetworkConnLog(TcpIpConnectTraceData obj)
        {
            //if (obj.ToString().Contains(monitored_Dir))
            //{
            //FileIOTraceEvent log = new FileIOTraceEvent("ProcessStop", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");
            //ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), "", "", obj.ProcessName, "");
            etwlog.AppendConsole1(_rtb1, "network", NetworkLog_ToString(obj), Color.RoyalBlue);
            if (etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName))
                etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            //}
        }
        public void GetNetworkLog(TcpIpTraceData obj)
        {
            //if (obj.ToString().Contains(monitored_Dir))
            //{
            //FileIOTraceEvent log = new FileIOTraceEvent("ProcessStop", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");
            //ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), "", "", obj.ProcessName, "");
            etwlog.AppendConsole1(_rtb1, "network", NetworkLog_ToString(obj), Color.RoyalBlue);
            //}
        }
        public string NetworkLog_ToString(TcpIpConnectTraceData obj)
        {
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                obj.ProcessName + "," +
                obj.ThreadID + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                obj.size + "," +
                obj.daddr.ToString() + "," +
                obj.saddr.ToString() + "," +
                obj.dport + "," +
                obj.sport + "," +
                obj.mss + "," +
                obj.sackopt + "," +
                obj.tsopt + "," +
                obj.wsopt + "," +
                obj.rcvwin + "," +
                obj.rcvwinscale + "," +
                obj.sndwinscale + "," +
                obj.seqnum + "," +
                obj.connid.ToString();
            return csvstr;
        }
        public string NetworkLog_ToString(TcpIpTraceData obj)
        {
            string csvstr = 
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                obj.ProcessName + "," +
                obj.ThreadID + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                obj.size + "," +
                obj.daddr.ToString() + "," +
                obj.saddr.ToString() + "," +
                obj.dport + "," +
                obj.sport + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                obj.seqnum + "," +
                obj.connid.ToString();
            return csvstr;
        }

        #endregion Network

        #region Thread
        public void Thread_Start(ThreadTraceData obj)
        {
            //FileIOTraceEvent log = new FileIOTraceEvent("ThreadStart", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                int ParentProcessIDCount = PlstEvents.Count(ev => ev.ParentProcessID == obj.ParentProcessID.ToString() && obj.ParentProcessID != -1);
                int ParentThreadIDCount = PlstEvents.Count(ev => ev.ParentThreadID == obj.ParentThreadID.ToString() && obj.ParentThreadID != -1);
                int ProcessIDCount = PlstEvents.Count(ev => ev.ProcessID == obj.ProcessID.ToString() && obj.ProcessID != -1);
                ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ParentProcessID.ToString(), obj.ParentThreadID.ToString(), obj.ProcessName, obj.ThreadName, ProcessIDCount, ParentProcessIDCount, ParentThreadIDCount);
                PlstEvents.Add(log);
                etwlog.AppendConsole1(_rtb1, "thread", log.ToString(), Color.Green);
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                //if ((obj.ProcessID != obj.ParentProcessID) && (obj.ParentProcessID.ToString() != "-1"))
                //{
                etwlog.addLog("thread", ThreadLog_ToString(obj));
                //etwlog.AppendConsole1(_rtb1, "thread", ThreadLog_ToString(obj), Color.Green);
                    if (!string.IsNullOrEmpty(obj.ParentThreadID.ToString())) etwlog.ParentThreadIDMapping.TryAdd(obj.ThreadID, obj.ParentThreadID);
                    if (!string.IsNullOrEmpty(obj.ProcessName.ToString())) etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                    if (!string.IsNullOrEmpty(obj.ParentProcessID.ToString())) etwlog.ParentProcessIDMapping.TryAdd(obj.ProcessID, obj.ParentProcessID);
                //if (obj.ProcessID == obj.ParentProcessID) MessageBox.Show(obj.ParentProcessID.ToString());
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
                // }
            }
        }
        public void Thread_Stop(ThreadTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (obj.ToString().Contains(ETW_Global.monitored_Dir))
                {
                    //FileIOTraceEvent log = new FileIOTraceEvent("ThreadStop", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");

                    int ParentProcessIDCount = PlstEvents.Count(ev => ev.ParentProcessID == obj.ParentProcessID.ToString() && obj.ParentProcessID != -1);
                    int ParentThreadIDCount = PlstEvents.Count(ev => ev.ParentThreadID == obj.ParentThreadID.ToString() && obj.ParentThreadID != -1);
                    int ProcessIDCount = PlstEvents.Count(ev => ev.ProcessID == obj.ProcessID.ToString() && obj.ProcessID != -1);
                    ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ParentProcessID.ToString(), obj.ParentThreadID.ToString(), obj.ProcessName, obj.ThreadName, ProcessIDCount, ParentProcessIDCount, ParentThreadIDCount);
                    etwlog.AppendConsole1(_rtb1, "thread", log.ToString(), Color.DarkSeaGreen);
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                etwlog.addLog("thread", ThreadLog_ToString(obj));
                //etwlog.AppendConsole1(_rtb1, "thread", ThreadLog_ToString(obj), Color.DarkSeaGreen);
            }
        }
        public string ThreadLog_ToString(ThreadTraceData obj)
        {
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," +
                obj.ProcessName + "," +
                obj.ThreadID + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                obj.ThreadName + "," +
                obj.StackBase.ToString() + "," +
                obj.StackLimit.ToString() + "," +
                obj.UserStackBase.ToString() + "," +
                obj.UserStackLimit.ToString() + "," +
                obj.StartAddr.ToString() + "," +
                obj.Win32StartAddr.ToString() + "," +
                obj.TebBase.ToString() + "," +
                obj.SubProcessTag + "," +
                obj.ParentProcessID + "," +
                obj.ParentThreadID;
            return csvstr;
        }
        #endregion Thread

        #region Process
        public void Process_Start(ProcessTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), "", "", obj.ProcessName, "");
                PlstEvents.Add(log);
                etwlog.AppendConsole1(_rtb1, "process", log.ToString(), Color.Red);
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                string log = ProcessLog_ToString(obj);
                etwlog.AppendConsole1(_rtb1, "process", log, Color.Blue);
                if (!string.IsNullOrEmpty(obj.ParentID.ToString())) etwlog.ParentProcessIDMapping.TryAdd(obj.ProcessID, obj.ParentID);
                if (!string.IsNullOrEmpty(obj.ImageFileName.ToString())) etwlog.ProcessImageFileName.TryAdd(obj.ProcessID, obj.ImageFileName);
                if (!string.IsNullOrEmpty(obj.ProcessName.ToString())) etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void Process_Stop(ProcessTraceData obj)
        {
            //if (obj.ToString().Contains(monitored_Dir))
            //{
            //FileIOTraceEvent log = new FileIOTraceEvent("ProcessStop", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, "", 0, 0, "", "");
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                ProcessIOTraceEvent log = new ProcessIOTraceEvent(obj.EventName, obj.TimeStampRelativeMSec.ToString(), obj.ProcessID.ToString(), obj.ThreadID.ToString(), "", "", obj.ProcessName, "");
                PlstEvents.Add(log);
                etwlog.AppendConsole1(_rtb1, "process", log.ToString(), Color.Red);
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!string.IsNullOrEmpty(obj.ParentID.ToString())) etwlog.ParentProcessIDMapping.TryAdd(obj.ProcessID, obj.ParentID);
                etwlog.AppendConsole1(_rtb1, "process", ProcessLog_ToString(obj), Color.RoyalBlue); 
            }
            //}
        }
        public string ProcessLog_ToString(ProcessTraceData obj)
        {
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," +
                obj.ProcessName + "," +
                obj.ThreadID + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                obj.ParentID + "," +
                obj.ImageFileName + "," +
                obj.DirectoryTableBase.ToString() + "," +
                obj.Flags.ToString() + "," +
                obj.SessionID + "," +
                obj.ExitStatus + "," +
                obj.UniqueProcessKey.ToString() + "," +
                obj.CommandLine.Replace(',','_');
            return csvstr;
        }
        #endregion Process

        #region FileIO

        public void FileIOTrace_Rename(string eventsCache, FileIOInfoTraceData obj)
        {            
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_RENAME)
                {
                    lstEvents.Add(new FileIOTraceEvent("", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey, obj.FileObject, "", ""));
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void FileIOTrace_FileCreate(string eventsCache, FileIONameTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_FILECREATE)
                {
                    if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
                    {
                        string ext = Path.GetExtension(obj.FileName);
                        if (!string.IsNullOrEmpty(ext))
                        {
                            var fileEvent = lstEvents.FindIndex(ev => ev.EventName == ETW_Events.EVENT_FILEIO_RENAME && ev.FileKey == obj.FileKey);
                            if (fileEvent != -1)
                            {
                                lstEvents[fileEvent].Remarks = "Rename";
                                lstEvents[fileEvent].NewFileName = Path.GetFileName(obj.FileName);
                                etwlog.AppendConsole1(_rtb1, "file", lstEvents[fileEvent].ToString(), Color.Yellow);
                                lstEvents.RemoveAt(lstEvents.FindIndex(ev => ev.FileKey == obj.FileKey));
                                //list cleanup (delete old entries)
                                int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                                if (sss >= 0) { lstEvents.RemoveAt(sss); }
                            }
                        }
                    }
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void FileIOTrace_FsControl(string eventsCache, FileIOInfoTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_FSCONTROL)
                {
                    string ext = Path.GetExtension(obj.FileName);
                    if (!string.IsNullOrEmpty(ext))
                    {
                        var fileEvent = lstEvents.FindIndex(ev => ev.EventName == ETW_Events.EVENT_FILEIO_RENAME && ev.FileKey == obj.FileKey);
                        if (fileEvent != -1)
                        {
                            lstEvents[fileEvent].NewFileName = Path.GetFileName(obj.FileName);
                            etwlog.AppendConsole1(_rtb1, "file", lstEvents[fileEvent].ToString(), Color.Yellow);
                            lstEvents.RemoveAt(lstEvents.FindIndex(ev => ev.FileKey == obj.FileKey));
                            //list cleanup (delete old entries)
                            int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                            if (sss >= 0) { lstEvents.RemoveAt(sss); }
                        }
                    }
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void FileIOTrace_FileDelete(string eventsCache, FileIONameTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_FILEDELETE)
                {
                    var fileEvent = lstEvents.FindIndex(ev => ev.EventName == ETW_Events.EVENT_FILEIO_RENAME && ev.FileKey == obj.FileKey);
                    if (fileEvent != -1)
                    {
                        lstEvents[fileEvent].Remarks = "FileMoved";
                        etwlog.AppendConsole1(_rtb1, "file", lstEvents[fileEvent].ToString(), Color.Yellow);
                        //lstEvents.RemoveAt(lstEvents.FindIndex(ev => ev.FileKey == obj.FileKey)); do not delete for rename filecreate event
                        //list cleanup (delete old entries)
                        int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                        if (sss >= 0) { lstEvents.RemoveAt(sss); }
                    }
                    else
                    {
                        FileIOTraceEvent log = new FileIOTraceEvent("FileDeleted", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey, 0, "", "");
                        etwlog.AppendConsole1(_rtb1, "file", log.ToString(), Color.Yellow);
                        //list cleanup (delete old entries)
                        int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                        if (sss >= 0) { lstEvents.RemoveAt(sss); }
                    }
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj) , Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void GetFileIO_Create(string eventsCache, FileIOCreateTraceData obj)   //NO FILEKEY IN FILECREATE EVENT
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_CREATE && obj.CreateDisposition == CreateDisposition.CREATE_NEW)
                {
                    FileIOTraceEvent log = new FileIOTraceEvent("Create", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, 0, obj.FileObject, obj.CreateDisposition.ToString(), obj.CreateOptions.ToString());
                    etwlog.AppendConsole1(_rtb1, "file", log.ToString(), Color.Violet);
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Violet);

                if (eventsCache == "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void GetFileIO_Delete(string eventsCache, FileIOInfoTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (obj.EventName == ETW_Events.EVENT_FILEIO_DELETE && obj.ExtraInfo.ToString() == "1")
                {
                    //lstEvents.Add(new FileIOTraceEvent(obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey.ToString()));
                    FileIOTraceEvent log = new FileIOTraceEvent("Delete", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey, obj.FileObject, "", "");
                    etwlog.AppendConsole1(_rtb1, "file", log.ToString(), Color.Violet);
                    //list cleanup (delete old entries)
                    int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                    if (sss >= 0) { lstEvents.RemoveAt(sss); }
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Violet);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void HandleFileIo_Write(string eventsCache, FileIOReadWriteTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global .FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                //filter duplicate event 
                var fileEvent = lstEvents.FindIndex(ev => ev.EventName == ETW_Events.EVENT_FILEIO_WRITE && ev.FileKey == obj.FileKey && obj.ProcessID < 10);
                if (fileEvent == -1)
                {
                    string operation = "Write";
                    FileIOTraceEvent log = new FileIOTraceEvent(operation, obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey, obj.FileObject, "", "");
                    etwlog.AppendConsole1(_rtb1, "file", log.ToString(), Color.Yellow);
                    //add eventg to list (for filtering duplicates)
                    lstEvents.Add(log);
                    //list cleanup (delete old entries)
                    int sss = lstEvents.FindIndex(ev => ev.TimeStamp < DateTime.Now.AddSeconds(-30));
                    if (sss >= 0) { lstEvents.RemoveAt(sss); }
                }
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void HandleFileIo_Read(string eventsCache, FileIOReadWriteTraceData obj)
        {
            if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_ALL)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                //Console.ForegroundColor = ConsoleColor.Magenta;
                FileIOTraceEvent log = new FileIOTraceEvent("Read", obj.ID.ToString(), obj.TimeStampRelativeMSec.ToString(), obj.EventName, obj.ProcessID.ToString(), obj.ProcessName, obj.FileName, obj.FileKey, obj.FileObject, "", "");
                etwlog.AppendConsole1(_rtb1, "file", log.ToString(), Color.Yellow);
            }
            else if (ETW_Global.EVENT_FILTERS == ETW_Global.FILTER_NONE)
            {
                if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
                foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

                if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

                etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
                //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

                etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
                //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
            }
        }
        public void HandleFileIo_SetInfo_QueryInfo(string eventsCache, FileIOInfoTraceData obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

            etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
            //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }
        public void HandleFileIo_DirEnum_NOTIFY(string eventsCache, FileIODirEnumTraceData obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

            etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
            //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }
        public void HandleFileIo_Close_Cleanup(string eventsCache, FileIOSimpleOpTraceData obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

            etwlog.FileList.TryAdd(obj.FileKey.ToString(), obj.FileName);
            //etwlog.AppendConsole2(_rtb2, "FileKey : " + obj.FileKey.ToString() + " FileName : " + obj.FileName);

            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }
        public void HandleFileIo_Flush(string eventsCache, FileIOSimpleOpTraceData obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb1, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }

        public void HandleFileIo_OpEnd(string eventsCache, FileIOOpEndTraceData obj)
        {
            //if (!obj.ToString().Contains(ETW_Monitor.monitored_Dir)) { return; }
            //foreach (string s in ETW_Monitor.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            //etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            //if (etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName))
            //    etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }

        public void HandleFileIo_FileRunDown(string eventsCache, FileIONameTraceData obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", FileIOLog_ToString(obj), Color.Yellow);

            if (eventsCache== "1") { etwlog.cachedEvents.AppendLine(FileIOLog_ToString(obj)); }

            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }

        public void Unhandled_CallBacks(TraceEvent obj)
        {
            if (!obj.ToString().Contains(ETW_Global.monitored_Dir)) { return; }
            foreach (string s in ETW_Global.excludedDirs) { if (obj.ToString().ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
            etwlog.AppendConsole1(_rtb1, "file", obj.ToString(), Color.Yellow);
            etwlog.ProcessList.TryAdd(obj.ProcessID, obj.ProcessName);
            //etwlog.AppendConsole2(_rtb2, "ProcessID : " + obj.ProcessID.ToString() + " ProcessName : " + obj.ProcessName);
        }

        #region stringoperations

        public string GetParentProcessID(int ProcessID)
        {
            int ppid = 0;
            if (etwlog.ParentProcessIDMapping.ContainsKey(ProcessID))
                ppid = etwlog.ParentProcessIDMapping[ProcessID];
            return ppid.ToString();
        }

        public string GetParentThreadID(int ThreadID)
        {
            int ptid = 0;
            if (etwlog.ParentThreadIDMapping.ContainsKey(ThreadID))
                ptid = etwlog.ParentThreadIDMapping[ThreadID];
            return ptid.ToString();
        }

        public string FileIOLog_ToString(FileIOOpEndTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," +
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," + 
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                "0" + "," +
                obj.IrpPtr.ToString() + "," +
                 "0" + "," +
                 "0" + "," +
                obj.ExtraInfo.ToString() + "," +
                "0";
            return csvstr;
        }

        public string FileIOLog_ToString(FileIONameTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," + 
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                "0" + "," +
                "0" + "," +
                obj.FileKey.ToString() + "," +
                "0" + "," +
                "0";
            return csvstr;
        }
        public string FileIOLog_ToString(FileIOReadWriteTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," + 
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                obj.IrpPtr.ToString() + "," +
                obj.FileObject.ToString() + "," +
                obj.FileKey.ToString() + "," +
                "0" + "," +
                "0";
            return csvstr;
        }
        public string FileIOLog_ToString(FileIOCreateTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                obj.IrpPtr.ToString() + "," +
                obj.FileObject.ToString() + "," +
                "0" + "," +
                "0" + "," +
                "0";
            return csvstr;
        }

        

        public string FileIOLog_ToString(FileIOInfoTraceData obj) 
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," + 
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                obj.IrpPtr.ToString() + "," +
                obj.FileObject.ToString() + "," +
                obj.FileKey.ToString() + "," +
                obj.ExtraInfo.ToString() + "," +
                obj.InfoClass.ToString();
            return csvstr;
        }
        public string FileIOLog_ToString(FileIODirEnumTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," +
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                obj.IrpPtr.ToString() + "," +
                obj.FileObject.ToString() + "," +
                obj.FileKey.ToString() + "," +
                "0" + "," +
                obj.InfoClass.ToString();
            return csvstr;
        }
        public string FileIOLog_ToString(FileIOSimpleOpTraceData obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," +
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                Path.GetDirectoryName(obj.FileName) + "," +
                Path.GetFileName(obj.FileName).Split(new[] { '.' }, 2)[0] + "," +
                Path.GetExtension(obj.FileName) + "," +
                obj.IrpPtr.ToString() + "," +
                obj.FileObject.ToString() + "," +
                obj.FileKey.ToString() + "," +
                "0" + "," +
                "0";
            return csvstr;
        }
        public string ToString(TraceEvent obj)
        {
            string ProcessName;
            if (string.IsNullOrEmpty(obj.ProcessName)) { ProcessName = "Empty"; } else { ProcessName = obj.ProcessName; }
            string csvstr =
                obj.TimeStamp.ToString("HH:mm:ss.ffff", System.Globalization.CultureInfo.InvariantCulture) + "," +
                obj.TimeStampRelativeMSec + "," +
                obj.ProcessID + "," + GetParentProcessID(obj.ProcessID) + "," +
                ProcessName + "," +
                obj.ThreadID + "," + GetParentThreadID(obj.ThreadID) + "," +
                obj.EventName + "," +
                ETW_Events.getEventID(obj.EventName) + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0" + "," +
                "0";
            return csvstr;
        }

        #endregion stringoperations

        #endregion FileIO

        #region FileIO2

        public void insert_FileIOLog(FileIOInfoTraceData obj)
        {
            insertlog(obj.FileName, obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ProcessName, obj.EventName);
        }

        public void insert_FileIOLog(FileIOCreateTraceData obj)
        {
            insertlog( obj.FileName, obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ProcessName, obj.EventName);
        }
        public void insert_FileIOLog(FileIOSimpleOpTraceData obj)
        {
            insertlog( obj.FileName, obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ProcessName, obj.EventName);
        }
        public void insert_FileIOLog(FileIOReadWriteTraceData obj)
        {
            insertlog( obj.FileName, obj.ProcessID.ToString(), obj.ThreadID.ToString(), obj.ProcessName,obj.EventName);
        }
        private void insertlog(string FileName, string ProcessID, string ThreadID, string ProcessName, string EventName)
        {
            try
            {
                foreach (string s in ETW_Global.excludedDirs) { if (FileName.ToLower().Contains(s, StringComparison.CurrentCultureIgnoreCase)) return; }
                if (string.IsNullOrEmpty(Path.GetFileName(FileName))) return;
                if (string.IsNullOrEmpty(ProcessID)) return;
                etwlog.AppendConsole1(_rtb1, FileName + " , " + ProcessID.ToString() + " , " + ProcessName + " , " + EventName, Color.Yellow);
                string eid = ETW_Events.getEventID(EventName);
                string fn = (Path.GetDirectoryName(FileName) + Path.DirectorySeparatorChar + Path.GetFileName(FileName).Split(new[] { '.' }, 2)[0]).Replace("'", "''");
                //-------------------------------------------------
                //DataRow[] found_keys = etwlog.fileIOLog.Select("FileKey = '" + FileKey + "'");
                DataRow[] found_fn = etwlog.fileIOLog.Select("fn = '" + fn + "'");
                if (found_fn.Any())
                {
                    string pn = ProcessName;
                    if (ETW_Global.benignProcessNames.Any(s => s == pn)) return;
                    string ft = (string)found_fn[0]["ft"];
                    string nft = Path.GetExtension(FileName);
                    string concatenatedEid = string.Empty;
                    foreach (DataRow r in found_fn)
                    {
                        concatenatedEid = r.Field<string>("eid") + concatenatedEid;
                    }
                    if (concatenatedEid.Length == 0 || concatenatedEid[concatenatedEid.Length - 1] != char.Parse(eid))
                    {
                        concatenatedEid = concatenatedEid + eid;
                    }
                    found_fn[0]["eid"] = concatenatedEid;
                    found_fn[0]["ts"] = DateTime.Now.Ticks;
                    if (ft != nft)
                    {
                        found_fn[0]["nft"] = nft;
                    }
                    etwlog.fileIOLog.AcceptChanges();
                    //--------------------------------------------------------------------------------------
                    if (etwlog.maliciousProcessNames.Contains(pn)) //check if process already marked malicious to prohibit multiple ransomware messages for same instance
                    {
                        killMaliciousProcess(ProcessID,pn);
                        string str = (string)found_fn[0]["fn"] + "," + ft + "," + nft;
                        if (!etwlog.infectedfilenames.Contains(str))
                        {
                            etwlog.infectedfilescounter++;
                            etwlog.infectedfilenames.Add(str);
                        }
                        return;
                    }
                    bool MaliciousPatternExists = checkIfPatternMalicious(concatenatedEid);
                    nft = (string)found_fn[0]["nft"];
                    //if (!eid.Contains("g")) 
                    //{
                    //    etwlog.AppendConsole2(_rtb2, fn + ", " + ft + ", " + nft + ", " + eid + ", " + concatenatedEid+" , "+ MaliciousPatternExists.ToString(), Color.YellowGreen);
                    //    return; 
                    //}
                    //--------------------------------------------------------------------------------------
                    if (MaliciousPatternExists == true && nft != "" && nft != ft)
                    {
                        System.Diagnostics.Process.GetProcessById(int.Parse(ProcessID)).Suspend();
                        double prevEntropy = (double)found_fn[0]["entropy"];
                        double newEntropy = ETW_Entropy.CalculateShannonEntropy(FileName, 10000);
                        etwlog.AppendConsole2(_rtb2, fn + " , " + ft + " , " + nft + " prevEntropy: " + prevEntropy + " newEntropy: " + newEntropy, Color.OrangeRed);
                        if (prevEntropy < 10 && newEntropy > prevEntropy)
                        {
                            etwlog.maliciousLogs.Rows.Add(found_fn[0].ItemArray);
                            DialogResult result = MessageBox.Show(pn + " seems Malicious. Do you want to terminate it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                if (!etwlog.maliciousProcessNames.Contains(pn))
                                { etwlog.maliciousProcessNames.Add(pn); }
                                killMaliciousProcess(ProcessID,pn);
                            }
                            else
                            {
                                ETW_Global.benignProcessNames.Add(pn);
                                System.Diagnostics.Process.GetProcessById(int.Parse(ProcessID)).Resume();
                            }
                        }
                        else { System.Diagnostics.Process.GetProcessById(int.Parse(ProcessID)).Resume(); }
                    }
                    return;
                }
                //-------------------------------------------------
                DataRow row = etwlog.fileIOLog.NewRow();
                if (string.IsNullOrEmpty(ProcessName)) { ProcessName = "Empty"; }
                row["ts"] = DateTime.Now.Ticks;
                row["pn"] = ProcessName;
                row["FileName"] = FileName; //Filename with ext
                row["fn"] = fn;             //FilePath + Filename with out ext
                row["pid"] = ProcessID;
                row["tid"] = ThreadID;
                row["eid"] = eid;
                row["ft"] = Path.GetExtension(FileName);    //ext
                row["nft"] = "";
                double entropy = ETW_Entropy.CalculateShannonEntropy(FileName, 10000);                
                row["entropy"] = entropy;
                etwlog.fileIOLog.Rows.Add(row);
                if (entropy == 100)
                {
                    etwlog.AppendConsole2(_rtb2, fn + Path.GetExtension(FileName) + " , entropy: " + entropy, Color.IndianRed);
                }
            }
            catch (Exception e)
            {
                etwlog.AppendConsole2(_rtb2, "\nException in Code : " + e.Message + "\n" + e.ToString(), Color.PaleVioletRed);
            }

        }
        private void killMaliciousProcess(string ProcessID, string pn)
        {
            try
            {
                System.Diagnostics.Process.GetProcessById(int.Parse(ProcessID)).Kill();
                etwlog.AppendConsole2(_rtb2, " Alert: Ransomware attack Detected.", Color.Red);
                etwlog.AppendConsole2(_rtb2, "'" + pn + "' killed;", Color.YellowGreen);
            }
            catch (Exception ex) 
            { 
                //etwlog.AppendConsole2(_rtb2, ex.Message, Color.Red);
            }            
            try//Confirm process kill.
            {
                System.Diagnostics.Process.GetProcessById(int.Parse(ProcessID)).Kill();
            }
            catch (Exception)
            {
                etwlog.AppendConsole2(_rtb2, "'" + pn + "' already killed;", Color.Green);
            }            
        }
        private bool checkIfPatternMalicious(string eid)
        {
            char[] eidArray = eid.ToLower().ToCharArray();
            char[] patternA = {  'a', 'c', 'd' };
            char[] patternB = {  'b', 'c', 'd' };
            bool matchesPatternA = patternA.All(c => eidArray.Contains(c));
            bool matchesPatternB = patternB.All(c => eidArray.Contains(c));
            return matchesPatternA || matchesPatternB;
        }    

        #endregion

        public void GetALLLog(TraceEvent obj)
        {
            if (obj.ToString().Contains(ETW_Global.monitored_Dir))
            {
                etwlog.AppendConsole2(_rtb1, obj.ToString());
            }
        }

    }
}

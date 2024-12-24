using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace MyFileIOMonitor
{
    internal class ETW_Collector
    {
        ETW_Callbacks callback;
        ETW_Logger etwlog;
        RichTextBox rtb2;
        TraceEventSession KernelSession;
        Thread t;
        public ETW_Collector(RichTextBox rtb, RichTextBox _rtb2, ETW_Logger _etwlog, TraceEventSession _KernelSession)
        {
            etwlog = _etwlog;
            rtb2 = _rtb2;
            KernelSession = _KernelSession;
            callback = new ETW_Callbacks(rtb, _rtb2, _etwlog);
        }
        public ETW_Collector(RichTextBox rtb, RichTextBox _rtb2, ETW_Logger _etwlog)
        {
            etwlog = _etwlog;
            rtb2 = _rtb2;
            callback = new ETW_Callbacks(rtb, _rtb2, _etwlog);
        }
        public ETW_Collector() { }
        public void Run()
        {
            TraceEventSession KernelSession = new TraceEventSession("sessionName");
            KernelSession.StopOnDispose = true;
            KernelSession.EnableKernelProvider(
                 KernelTraceEventParser.Keywords.FileIOInit      //required reade/write/create/delete operations
               | KernelTraceEventParser.Keywords.DiskFileIO    //required for rename operation
                                                               //| KernelTraceEventParser.Keywords.NetworkTCPIP
                                                               //| KernelTraceEventParser.Keywords.Registry
                                                               //| KernelTraceEventParser.Keywords.Thread
               | KernelTraceEventParser.Keywords.Process
               );
            //if (ETW_Monitor.file)
            //{
            //// KernelSession.EnableKernelProvider(
            ////  KernelTraceEventParser.Keywords.FileIOInit      //required reade/write/create/delete operations
            ////| KernelTraceEventParser.Keywords.DiskFileIO    //required for rename operation
            ////);
            // //Create
            // //KernelSession.Source.Kernel.FileIOCreate += callback.GetFileIO_Create;
            // //Write
            // KernelSession.Source.Kernel.FileIOWrite += callback.HandleFileIo_Write;           //FileIOInit
            // //Read
            // KernelSession.Source.Kernel.FileIORead += callback.HandleFileIo_Read;             //FileIOInit
            // //Rename
            // KernelSession.Source.Kernel.FileIORename += callback.FileIOTrace_Rename;          //FileIOInit
            // KernelSession.Source.Kernel.FileIOFSControl += callback.FileIOTrace_FsControl;    //for getting new file name (script)
            // KernelSession.Source.Kernel.FileIOFileCreate += callback.FileIOTrace_FileCreate;  //for getting new file name (User Action)
            // KernelSession.Source.Kernel.FileIOFileDelete += callback.FileIOTrace_FileDelete;  //DiskFileIO - for detecting file moved or permanent deletion case
            // //Delete
            // KernelSession.Source.Kernel.FileIODelete += callback.GetFileIO_Delete;            //FileIOInit
            //                                                                                   //
            //KernelSession.Source.Kernel.All += callback.FileIOLog;
            //}
            //if (ETW_Monitor.process)
            //{
            //    //KernelSession.EnableKernelProvider(KernelTraceEventParser.Keywords.Process);
            //    KernelSession.Source.Kernel.ProcessStart += callback.Process_Start;
            //    KernelSession.Source.Kernel.ProcessStop += callback.Process_Stop;
            //}
            //if (ETW_Monitor.thread)
            //{
            //    //KernelSession.EnableKernelProvider(KernelTraceEventParser.Keywords.Thread);
            //    KernelSession.Source.Kernel.ThreadStart += callback.Thread_Start;
            //    KernelSession.Source.Kernel.ThreadStop += callback.Thread_Stop;
            //}
            //if (ETW_Monitor.reg)
            //{
            //    //KernelSession.EnableKernelProvider(KernelTraceEventParser.Keywords.Registry);
            //    KernelSession.Source.Kernel.RegistryCreate += callback.monitor_reg;
            //    KernelSession.Source.Kernel.RegistryDelete += callback.monitor_reg;
            //    KernelSession.Source.Kernel.RegistrySetValue += callback.monitor_reg;
            //    KernelSession.Source.Kernel.RegistryDeleteValue += callback.monitor_reg;
            //}
            //if (ETW_Monitor.network)
            //{
            //    //KernelSession.EnableKernelProvider(KernelTraceEventParser.Keywords.NetworkTCPIP);
            //    KernelSession.Source.Kernel.TcpIpConnect += callback.GetNetworkConnLog;
            //    KernelSession.Source.Kernel.TcpIpDisconnect += callback.GetNetworkLog;
            //}
            ////All
            //if (ETW_Monitor.all)
            //{                
            KernelSession.Source.Kernel.All += HandleCallBacks;
            //}
            //etwlog.AppendConsole1(_rtb ,"", "****** Start listening for events from selected providers.******", Color.Black);
            //Start Processing Events
            if (!KernelSession.Source.Process())
            {
                KernelSession.Dispose();
            }
        }
        public void Run1()
        {
            KernelSession.Source.Kernel.All += HandleCallBacks;
            etwlog.AppendConsole2(rtb2, "*** RansomGuard Started.***");
            if (!KernelSession.Source.Process())
            {
                KernelSession.Dispose();
            }
        }
        public void Run2()
        {
            KernelSession.Source.Kernel.All += HandleCallBacks2;
            etwlog.AppendConsole2(rtb2, "*** RansomGuard Started.***");
            etwlog.AppendConsole2(rtb2, "Monitoring for malicious file events.....");
            if (!KernelSession.Source.Process())
            {
                KernelSession.Dispose();
            }
        }
        public void HandleCallBacks2(TraceEvent obj)
        {
            switch (obj.EventName)
            {
                //case (ETW_Events.EVENT_FILEIO_SETINFO): callback.insert_FileIOLog((FileIOInfoTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_CREATE):  callback.insert_FileIOLog( (FileIOCreateTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_READ):   callback.insert_FileIOLog( (FileIOReadWriteTraceData)obj);break;
                case (ETW_Events.EVENT_FILEIO_WRITE):  callback.insert_FileIOLog( (FileIOReadWriteTraceData)obj);break;
                case (ETW_Events.EVENT_FILEIO_DELETE): callback.insert_FileIOLog( (FileIOInfoTraceData)obj);break;
                case (ETW_Events.EVENT_FILEIO_RENAME): callback.insert_FileIOLog( (FileIOInfoTraceData)obj);break;
                case (ETW_Events.EVENT_FILEIO_CLOSE): callback.insert_FileIOLog((FileIOSimpleOpTraceData)obj); break;
            }
        }
        public void ProcessEtl(ETWTraceEventSource source, ToolStripStatusLabel lbl)
        {
            lbl.Text = "Processing Events from selected ETL File....";
            source.Process();
            lbl.Text = "Completed Processing Events.";
        }
        public void HandleCallBacks(TraceEvent obj)
        {            
            string eventsCache = "1";
            switch (obj.EventName)
            {
                case ETW_Events.EVENT_FILEIO_CREATE: logFileIOEvent(obj); callback.GetFileIO_Create(eventsCache, (FileIOCreateTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_READ): logFileIOEvent(obj); callback.HandleFileIo_Read(eventsCache, (FileIOReadWriteTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_WRITE): logFileIOEvent(obj); callback.HandleFileIo_Write(eventsCache, (FileIOReadWriteTraceData)obj); break;
                // case (ETW_Events.EVENT_FILEIO_FILECREATE): logFileIOEvent(obj); callback.FileIOTrace_FileCreate((FileIONameTraceData)obj); break;
                // case (ETW_Events.EVENT_FILEIO_FILEDELETE): logFileIOEvent(obj); callback.FileIOTrace_FileDelete((FileIONameTraceData)obj); break;
                // case (ETW_Events.EVENT_FILEIO_DIRENUM): logFileIOEvent(obj); callback.HandleFileIo_DirEnum_NOTIFY((FileIODirEnumTraceData)obj); break;
                //case (ETW_Events.EVENT_FILEIO_DIRNOTIFY): logFileIOEvent(obj); callback.HandleFileIo_DirEnum_NOTIFY(eventsCache, (FileIODirEnumTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_CLOSE): logFileIOEvent(obj); callback.HandleFileIo_Close_Cleanup(eventsCache, (FileIOSimpleOpTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_CLEANUP): logFileIOEvent(obj); callback.HandleFileIo_Close_Cleanup(eventsCache, (FileIOSimpleOpTraceData)obj); break;
                //case (ETW_Events.EVENT_FILEIO_SETINFO): logFileIOEvent(obj); callback.HandleFileIo_SetInfo_QueryInfo(eventsCache, (FileIOInfoTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_QUERYINFO): logFileIOEvent(obj); callback.HandleFileIo_SetInfo_QueryInfo(eventsCache, (FileIOInfoTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_DELETE): logFileIOEvent(obj); callback.GetFileIO_Delete(eventsCache, (FileIOInfoTraceData)obj); break;
                case (ETW_Events.EVENT_FILEIO_RENAME): logFileIOEvent(obj); callback.FileIOTrace_Rename(eventsCache, (FileIOInfoTraceData)obj); break;
                //case (ETW_Events.EVENT_FILEIO_FSCONTROL): logFileIOEvent(obj); callback.FileIOTrace_FsControl(eventsCache, (FileIOInfoTraceData)obj); break;
                //case (ETW_Events.EVENT_FILEIO_FLUSH): logFileIOEvent(obj); callback.HandleFileIo_Flush(eventsCache, (FileIOSimpleOpTraceData)obj); break;
                //case (ETW_Events.EVENT_FILEIO_OPERATIONEND): logFileIOEvent(obj); callback.HandleFileIo_OpEnd(eventsCache, (FileIOOpEndTraceData)obj); break;
                // case (ETW_Events.EVENT_FILEIO_FILERUNDOWN): logFileIOEvent(obj); callback.HandleFileIo_FileRunDown((FileIONameTraceData)obj); break;

                case (ETW_Events.EVENT_THREAD_DCSTART): logEvent(obj); break;
                case (ETW_Events.EVENT_THREAD_START): logEvent(obj); callback.Thread_Start((ThreadTraceData)obj); break;
                case (ETW_Events.EVENT_THREAD_STOP): logEvent(obj); callback.Thread_Stop((ThreadTraceData)obj); break;
                case (ETW_Events.EVENT_THREAD_SETNAME): logEvent(obj); break;

                case (ETW_Events.EVENT_PROCESS_DCSTART): logEvent(obj); break;
                case (ETW_Events.EVENT_PROCESS_START): logEvent(obj); callback.Process_Start((ProcessTraceData)obj); break;
                case (ETW_Events.EVENT_PROCESS_STOP): logEvent(obj); callback.Process_Stop((ProcessTraceData)obj); break;

                //case (ETW_Events.EVENT_REGISTRY_CREATE): logEvent(obj); callback.monitor_reg((RegistryTraceData)obj); break;
                //case (ETW_Events.EVENT_REGISTRY_DELETEVALUE): logEvent(obj); callback.monitor_reg((RegistryTraceData)obj); break;
                //case (ETW_Events.EVENT_REGISTRY_OPEN): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_DELETE): logEvent(obj); callback.monitor_reg((RegistryTraceData)obj); break;
                //case (ETW_Events.EVENT_REGISTRY_CLOSE): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_QUERY): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_QUERYVALUE): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_QUERYMULTIPLEVALUE): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_SETINFO): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_SETVALUE): logEvent(obj); callback.monitor_reg((RegistryTraceData)obj); break;
                //case (ETW_Events.EVENT_REGISTRY_ENAMURATEKEY): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_ENAMURATEVALUEKEY): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_KCBCREATE): logEvent(obj); break;
                //case (ETW_Events.EVENT_REGISTRY_KCBDELETE): logEvent(obj); break;

                //case (ETW_Events.EVENTTRACE_RUNDOWNCOMPLETE): logEvent(obj); break;
                //case (ETW_Events.EVENTTRACE_EXTENSION): logEvent(obj); break;
                //case (ETW_Events.EVENTTRACE_ENDEXTENSION): logEvent(obj); break;

                //case (ETW_Events.NETWORK_SYSTEMCONFIG): logEvent(obj); break;
                //case (ETW_Events.NETWORK_UDPIP_SEND): logEvent(obj); break;
                //case (ETW_Events.NETWORK_UDPIP_RCV): logEvent(obj); break;
                //case (ETW_Events.NETWORK_UDPIP_SEND_V6): logEvent(obj); break;
                //case (ETW_Events.NETWORK_UDPIP_RCV_V6): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_SEND): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_RCV): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_CONNECT): logEvent(obj); callback.GetNetworkConnLog((TcpIpConnectTraceData)obj); break;
                //case (ETW_Events.NETWORK_TCPIP_DISCONNECT): logEvent(obj); callback.GetNetworkLog((TcpIpTraceData)obj); break;
                //case (ETW_Events.NETWORK_TCPIP_TCPCOPY): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_RETRANSMIT): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_RECONNECT): logEvent(obj); break;
                //case (ETW_Events.NETWORK_TCPIP_ACCEPT): logEvent(obj); break;

                default: logEvent(obj); /*etwlog.AppendConsole2(rtb2, obj.EventName);Unhandled_CallBacks(obj);*/ break;
            }
        }
        private void logFileIOEvent(TraceEvent obj)
        {
            if (obj.ToString().Contains(ETW_Global.monitored_Dir))
            {
                //etwlog.addLog("event", +obj.TimeStampRelativeMSec + "," + obj.ProcessID + "," + obj.ThreadID + "," + ETW_Events.getEventID(obj.EventName));
            }
        }
        private void logEvent(TraceEvent obj)
        {
            //etwlog.addLog("event", +obj.TimeStampRelativeMSec + "," + obj.ProcessID + "," + obj.ThreadID + "," + ETW_Events.getEventID(obj.EventName));
        }
    }
}

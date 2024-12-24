
namespace MyFileIOMonitor
{
    internal class ETW_Events
    {
        public const string EVENT_FILEIO_RENAME =       "FileIO/Rename";
        public const string EVENT_FILEIO_DELETE =       "FileIO/Delete";
        public const string EVENT_FILEIO_READ =         "FileIO/Read";
        public const string EVENT_FILEIO_WRITE =        "FileIO/Write";
        public const string EVENT_FILEIO_SETINFO =      "FileIO/SetInfo";
        public const string EVENT_FILEIO_CREATE =       "FileIO/Create";
        public const string EVENT_FILEIO_CLOSE =        "FileIO/Close";
        public const string EVENT_FILEIO_FILECREATE =   "FileIO/FileCreate";
        public const string EVENT_FILEIO_FILEDELETE =   "FileIO/FileDelete";
        public const string EVENT_FILEIO_FSCONTROL =    "FileIO/FSControl";
        public const string EVENT_FILEIO_DIRENUM =      "FileIO/DirEnum";
        public const string EVENT_FILEIO_CLEANUP =      "FileIO/Cleanup";
        public const string EVENT_FILEIO_DIRNOTIFY =    "FileIO/DirNotify";
        public const string EVENT_FILEIO_QUERYINFO =    "FileIO/QueryInfo";
        public const string EVENT_FILEIO_FLUSH =        "FileIO/Flush";
        public const string EVENT_FILEIO_OPERATIONEND = "FileIO/OperationEnd";
        public const string EVENT_FILEIO_FILERUNDOWN =  "FileIO/FileRundown";

        public const string EVENT_THREAD_DCSTART =      "Thread/DCStart";
        public const string EVENT_THREAD_START =        "Thread/Start";
        public const string EVENT_THREAD_STOP =         "Thread/Stop";
        public const string EVENT_THREAD_SETNAME =      "Thread/SetName";

        public const string EVENT_PROCESS_DCSTART =     "Process/DCStart";
        public const string EVENT_PROCESS_START =       "Process/Start";
        public const string EVENT_PROCESS_STOP =        "Process/Stop";

        public const string EVENT_REGISTRY_SETVALUE =   "Registry/SetValue";
        public const string EVENT_REGISTRY_SETINFO =    "Registry/SetInformation";
        public const string EVENT_REGISTRY_ENAMURATEKEY = "Registry/EnumerateKey";
        public const string EVENT_REGISTRY_ENAMURATEVALUEKEY = "Registry/EnumerateValueKey";
        public const string EVENT_REGISTRY_OPEN =       "Registry/Open";
        public const string EVENT_REGISTRY_CLOSE =      "Registry/Close";
        public const string EVENT_REGISTRY_QUERYVALUE = "Registry/QueryValue";
        public const string EVENT_REGISTRY_QUERYMULTIPLEVALUE = "Registry/QueryMultipleValue";
        public const string EVENT_REGISTRY_QUERY =      "Registry/Query";
        public const string EVENT_REGISTRY_KCBCREATE =  "Registry/KCBCreate";
        public const string EVENT_REGISTRY_KCBDELETE =  "Registry/KCBDelete";
        public const string EVENT_REGISTRY_CREATE =     "Registry/Create";
        public const string EVENT_REGISTRY_DELETEVALUE = "Registry/DeleteValue";
        public const string EVENT_REGISTRY_DELETE =     "Registry/Delete";
        public const string EVENT_REGISTRY_FLUSH =      "Registry/Flush";

        public const string EVENTTRACE_RUNDOWNCOMPLETE = "EventTrace/RundownComplete";
        public const string EVENTTRACE_EXTENSION =      "EventTrace/Extension";
        public const string EVENTTRACE_ENDEXTENSION =   "EventTrace/EndExtension";

        public const string NETWORK_SYSTEMCONFIG =      "SystemConfig/Network";
        public const string NETWORK_UDPIP_SEND =        "UdpIp/Send";
        public const string NETWORK_UDPIP_RCV =         "UdpIp/Recv";
        public const string NETWORK_UDPIP_SEND_V6 =     "UdpIp/SendIPV6";
        public const string NETWORK_UDPIP_RCV_V6 =      "UdpIp/RecvIPV6";
        public const string NETWORK_TCPIP_SEND_V6 =     "TcpIp/SendIPV6";
        public const string NETWORK_TCPIP_RCV_V6 =      "TcpIp/RecvIPV6";
        public const string NETWORK_TCPIP_SEND =        "TcpIp/Send";
        public const string NETWORK_TCPIP_RCV =         "TcpIp/Recv";
        public const string NETWORK_TCPIP_CONNECT=      "TcpIp/Connect";
        public const string NETWORK_TCPIP_RECONNECT =   "TcpIp/Reconnect";
        public const string NETWORK_TCPIP_DISCONNECT =  "TcpIp/Disconnect";
        public const string NETWORK_TCPIP_TCPCOPY =     "TcpIp/TCPCopy";
        public const string NETWORK_TCPIP_RETRANSMIT =  "TcpIp/Retransmit";
        public const string NETWORK_TCPIP_ACCEPT =      "TcpIp/Accept";


        public static string getEventID(string EventName) 
        {
            string EID = "0";
            switch (EventName) 
            {
                case "FileIO/Rename":       EID = "a"; break;
                case "FileIO/Delete":       EID = "b"; break;
                case "FileIO/Read":         EID = "c"; break;
                case "FileIO/Write":        EID = "d"; break;
                case "FileIO/SetInfo":      EID = "e"; break;
                case "FileIO/Create":       EID = "f"; break;
                case "FileIO/Close":        EID = "g"; break;
                case "FileIO/FileCreate":   EID = "h"; break;
                case "FileIO/FileDelete":   EID = "i"; break;
                case "FileIO/FSControl":    EID = "j"; break;
                case "FileIO/DirEnum":      EID = "k"; break;
                case "FileIO/Cleanup":      EID = "l"; break;
                case "FileIO/DirNotify":    EID = "m"; break;
                case "FileIO/QueryInfo":    EID = "n"; break;
                case "FileIO/Flush":        EID = "o"; break;
                case "FileIO/OperationEnd": EID = "p"; break;
                case "FileIO/FileRundown":  EID = "q"; break;

                case "Thread/DCStart":      EID = "T101"; break;
                case "Thread/Start":        EID = "T102"; break;
                case "Thread/Stop":         EID = "T103"; break;
                case "Thread/SetName":      EID = "T104"; break;

                case "Process/DCStart":     EID = "P201"; break;
                case "Process/Start":       EID = "P202"; break;
                case "Process/Stop":        EID = "P203"; break;

                case "EventTrace/RundownComplete":  EID = "301"; break;
                case "EventTrace/Extension":        EID = "302"; break;
                case "EventTrace/EndExtension":     EID = "303"; break;

                case "Registry/SetValue":           EID = "R401"; break;
                case "Registry/SetInformation":     EID = "R402"; break;
                case "Registry/EnumerateKey":       EID = "R403"; break;
                case "Registry/EnumerateValueKey":  EID = "R404"; break;
                case "Registry/Open":               EID = "R405"; break;
                case "Registry/Close":              EID = "R406"; break;
                case "Registry/QueryValue":         EID = "R407"; break;
                case "Registry/QueryMultipleValue": EID = "R408"; break;
                case "Registry/Query":              EID = "R409"; break;
                case "Registry/KCBCreate":          EID = "R410"; break;
                case "Registry/KCBDelete":          EID = "R411"; break;
                case "Registry/Create":             EID = "R412"; break;
                case "Registry/DeleteValue":        EID = "R413"; break;
                case "Registry/Delete":             EID = "R414"; break;
                case "Registry/Flush":              EID = "R415"; break;

                case "SystemConfig/Network":        EID = "N501"; break;
                case "UdpIp/Send":                  EID = "N502"; break;
                case "UdpIp/Recv":                  EID = "N503"; break;
                case "UdpIp/SendIPV6":              EID = "N504"; break;
                case "UdpIp/RecvIPV6":              EID = "N505"; break;
                case "TcpIp/Send":                  EID = "N506"; break;
                case "TcpIp/Recv":                  EID = "N507"; break;
                case "TcpIp/Connect":               EID = "N508"; break;
                case "TcpIp/Reconnect":             EID = "N509"; break;
                case "TcpIp/Disconnect":            EID = "N510"; break;
                case "TcpIp/TCPCopy":               EID = "N511"; break;
                case "TcpIp/Retransmit":            EID = "N512"; break;
                case "TcpIp/Accept":                EID = "N513"; break;
                case "TcpIp/SendIPV6":              EID = "N514"; break;
                case "TcpIp/RecvIPV6":              EID = "N515"; break;
                default:                            EID = "0";   break;
            }          

            return EID; 
        }
    }
}

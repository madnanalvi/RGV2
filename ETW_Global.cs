
namespace MyFileIOMonitor
{
    internal class ETW_Global
    {
        public static bool VERBOSE1 = false;
        public static bool VERBOSE2 = true;
        public static string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;        
        public static string etlFilePath = "c:\\log.etl";
        public static bool isInferenceMode = false;
        public static string EVENT_FILTERS = FILTER_NONE;
        public const string FILTER_NONE = "APPLY NO FILTERS";
        public const string FILTER_ALL = "APPLY ALL EVENT FILTERS";
        public static List<string> benignProcessNames = new List<string>();
        public static List<string> excludedDirs = new List<string>();
        public static bool file1 = true, process1 = false, thread1 = false, reg1 = false, network1 = false, all1 = false;
        public static string inference_Script = baseDir + "py\\inference.pyw";
        public static string model = baseDir+ Properties.Settings.Default.model;
        public static int MALICIOUS_THRESHOLD = Properties.Settings.Default.MALICIOUS_THRESHOLD; 
        public static int ATTACK_THRESHOLD = Properties.Settings.Default.ATTACK_THRESHOLD;
        public static string pythonPath = Properties.Settings.Default.pythonPath;
        public static string monitored_Dir = Properties.Settings.Default.monitored_Dir;
        public static string verbose = Properties.Settings.Default.verbose;
        public static bool deleteCSVFiles = Properties.Settings.Default.deleteCSVFiles;
        public static int CACHESIZE = Properties.Settings.Default.cacheSize;
        public static int TIMER = Properties.Settings.Default.TIMER;
        public static string vocab = Properties.Settings.Default.VOCABULARY;
        public static Dictionary<string, List<string>> predictedScores = new Dictionary<string, List<string>>();
    }
}

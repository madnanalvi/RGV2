using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace MyFileIOMonitor
{
    public partial class ETW_Main : Form
    {
        public static bool file1 = false, process1 = false, thread1 = false, reg1 = false, network1 = false, all1 = false;
        ETW_Collector etw;
        ETW_Logger etwlog;
        StringBuilder events;
        TraceEventSession KernelSession;
        ETWTraceEventSource source;
        Thread t1;
        string csvpath = "";
        int SampleID = 0;
        public ETW_Main()
        {
            InitializeComponent();
            timer1.Interval = ETW_Global.TIMER;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            StartETW();
        }

        private void StartETW()
        {
            checkBox_csv.Enabled = false;
            if (checkBox_csv.Checked)
            {
                etwlog.AppendConsole2(log2, DateTime.Now.ToLongTimeString());
                etwlog.AppendConsole2(log2, "Started Logging ETW events to CSV file");
                etwlog.AppendConsole2(log2, "CSV file : " + csvpath);
                events = new StringBuilder();
            }
            else
            {
                etwlog.AppendConsole2(log2, "Python :  " + ETW_Global.pythonPath);
                etwlog.AppendConsole2(log2, "Model :   " + ETW_Global.model);
                etwlog.AppendConsole2(log2, "Vocabulary : " + ETW_Global.vocab);
            }
            ETW_Global.isInferenceMode = true;
            if (!(TraceEventSession.IsElevated() ?? false))
            {
                etwlog.AppendConsole2(log2, "To turn on ETW events you need to be Administrator, please run from an Admin process.");
            }
            else
            {
                string sessionName = KernelTraceEventParser.KernelSessionName;
                KernelSession = new TraceEventSession(sessionName);
                KernelSession.StopOnDispose = true;
                KernelSession.EnableKernelProvider(
                     KernelTraceEventParser.Keywords.FileIOInit      //required reade/write/create/delete operations
                   | KernelTraceEventParser.Keywords.DiskFileIO    //required for rename operation
                                                                   //| KernelTraceEventParser.Keywords.NetworkTCPIP
                                                                   //| KernelTraceEventParser.Keywords.Registry
                   | KernelTraceEventParser.Keywords.Thread
                   | KernelTraceEventParser.Keywords.Process
                    );
                etw = new ETW_Collector(log, log2, etwlog, KernelSession);
                t1 = new Thread(() => etw.Run1());
                t1.Start();
                button_start.Enabled = false;
                button_stop.Enabled = true;
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            stopETW();
        }
        public void stopETW()
        {
            checkBox_csv.Enabled = true;
            if (checkBox_csv.Checked)
            {
                etwlog.AppendConsole2(log2, "Stopped Logging ETW events to CSV file");
                etwlog.AppendConsole2(log2, "CSV file : " + csvpath);
                etwlog.AppendConsole2(log2, DateTime.Now.ToLongTimeString());
            }
            ETW_Global.isInferenceMode = false;
            try
            {
                if (t1.IsAlive)
                {
                    KernelSession.Dispose();
                    Thread.Sleep(1000);
                    //etw.StopETW(); 
                }
                if (!t1.IsAlive)
                {
                    etwlog.savelogFiles();
                    log2.AppendText("\n**  RansomGuard Stopped.  **\n");
                    //UncheckAllItems();
                    button_start.Enabled = true;
                    button_stop.Enabled = false;
                    //checkedListBox1.Enabled = true;
                    etw = new ETW_Collector();
                    etwlog = new ETW_Logger();
                    events = new StringBuilder();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void ETW_Main_Load(object sender, EventArgs e)
        {
            file1 = true;
            etwlog = new ETW_Logger();
            csvpath = ETW_Global.baseDir + "log\\events_" + DateTime.Now.Ticks.ToString() + ".csv";
            ETW_Global.excludedDirs.Add(@"C:\$Recycle.Bin".ToLower());
            ETW_Global.excludedDirs.Add(System.IO.Directory.GetCurrentDirectory().ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\appdata\\Local\\Microsoft\\WindowsApps\\");
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.System).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\WinSxS".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Logs".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Temp".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Servicing".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\assembly".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\ServiceProfiles".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\appdata".ToLower());
            if (!IsElevated)
            {
                string message = "To turn on ETW events you need to be Administrator, please run from an Admin process.";
                string title = "Administrator privilages required.";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    // Do something
                }
            }
        }
        public bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void buttonPyScript_Click(object sender, EventArgs e)
        {
            textBox_msg.Text = etwlog.cachedEvents.getLineCount().ToString();
            //bool hash = ETW_Analyze.isHavingDigitalSignatures(int.Parse(textBox1.Text));
            //MessageBox.Show(hash.ToString());

        }
        private static void EndProcessTree(int pid)
        {
            string Arguments = $"/pid {pid} /f /t";
            Process.Start("taskkill", Arguments).WaitForExit();
        }
        private void checkBox_verbose_CheckedChanged(object sender, EventArgs e)
        {
            ETW_Global.VERBOSE1 = checkBox_verbose.Checked;

        }

        private void button_exploreDir_Click(object sender, EventArgs e)
        {

            OpenFolder(ETW_Global.baseDir);
        }
        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }

        private void button_clr_Click(object sender, EventArgs e)
        {
            log.Clear();
            log2.Clear();
            System.GC.Collect();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ETW_Global.VERBOSE2 = checkBox_verbose2.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ETW_Global.isInferenceMode)
            {
                textBox_msg.Text = etwlog.cachedEvents.getLineCount().ToString();
                if (!checkBox_csv.Checked)
                {
                    analyzeEvents();
                }
                else
                {
                    saveCSV(csvpath);
                }
            }
        }

        private void saveCSV(string csvpath)
        {
            if (etwlog.cachedEvents.getLineCount() >= ETW_Global.CACHESIZE)
            {
                try
                {
                    System.IO.File.AppendAllText(csvpath, etwlog.cachedEvents.ToString());
                    events.AppendLine(etwlog.cachedEvents.ToString());
                    etwlog.cachedEvents = new cacheEvents();
                }
                catch (Exception) { }
            }
        }

        private void analyzeEvents()
        {
            log2.AppendText(".");
            if (etwlog.cachedEvents.getLineCount() >= ETW_Global.CACHESIZE)
            {
                etwlog.AppendConsole2(log2, SampleID.ToString() + ": " + etwlog.cachedEvents.getLineCount().ToString());
                try
                {
                    string csvpath = ETW_Global.baseDir + "log\\events_" + DateTime.Now.Ticks.ToString() + ".csv";
                    System.IO.File.WriteAllText(csvpath, etwlog.cachedEvents.ToString());
                    ETW_Analyze ml = new ETW_Analyze(log, log2, etwlog, csvpath, textBox_msg, SampleID);
                    new Thread(() => ml.analyzeEvents()).Start();
                    etwlog.cachedEvents = new cacheEvents();
                    etwlog.cachedEvents.AppendLine(etwlog.FileIOLogFile_Headers());
                    SampleID = SampleID + 1;
                }
                catch (Exception) { }
            }
        }



        private void button_hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ETW_Monitor m = new ETW_Monitor();
            m.Show();
        }

        private void checkBox_debug_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_debug.Checked) ETW_Global.verbose = "1"; else ETW_Global.verbose = "0";
        }

        private void ETW_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void checkBox_csv_CheckedChanged(object sender, EventArgs e)
        {
            csvpath = ETW_Global.baseDir + "log\\events_" + DateTime.Now.Ticks.ToString() + ".csv";
        }

        private void label_analyze_Click(object sender, EventArgs e)
        {
            //Analyzing events from previously saved CSV file
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ETW_Global.deleteCSVFiles = false;
                etwlog.AppendConsole2(log2, "File Path: " + openFileDialog1.FileName);
                etwlog.AppendConsole2(log2, "Analyzing events from CSV file");
                ETW_Analyze ml = new ETW_Analyze(log, log2, etwlog, openFileDialog1.FileName, textBox_msg, SampleID);
                new Thread(() => ml.analyzeEvents()).Start();
            }
        }

        private void label_save_Click(object sender, EventArgs e)
        {
            //backup method to save if log file is also encrypted
            try
            {
                string csvbkp = ETW_Global.baseDir + "log\\events_" + DateTime.Now.Ticks.ToString() + "_bkp.csv";
                System.IO.File.AppendAllText(csvbkp, events.ToString());
                //events = new StringBuilder();
            }
            catch (Exception) { }
        }
    }
}

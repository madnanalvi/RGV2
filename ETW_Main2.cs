using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Data;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace MyFileIOMonitor
{
    public partial class ETW_Main2 : Form
    {
        ETW_Collector etw;
        ETW_Logger etwlog;
        TraceEventSession KernelSession;
        Thread t1;
        public ETW_Main2()
        {
            InitializeComponent();
            timer1.Interval = ETW_Global.TIMER;
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            etwlog.infectedfilescounter = 0;
            StartETW();
            //try { timer1.Enabled = true; } catch (Exception) { }
        }

        private void StartETW()
        {
            etwlog.AppendConsole2(log2, DateTime.Now.ToLongTimeString());
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
                   | KernelTraceEventParser.Keywords.Thread
                   | KernelTraceEventParser.Keywords.Process
                    );
                etw = new ETW_Collector(log, log2, etwlog, KernelSession);
                t1 = new Thread(() => etw.Run2());
                t1.Start();
                button_start.Enabled = false;
                button_stop.Enabled = true;
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            stopETW();
            //try { timer1.Enabled = false; } catch (Exception) { }
        }
        public void stopETW()
        {
            etwlog.AppendConsole2(log2, DateTime.Now.ToLongTimeString());
            log2.AppendText("Infected Files: " + etwlog.infectedfilescounter.ToString());
            try
            {
                if (t1.IsAlive)
                {
                    KernelSession.Dispose();
                    Thread.Sleep(1000);
                }
                if (!t1.IsAlive)
                {
                    log2.AppendText("\n**  RansomGuard Stopped.  **");
                    button_start.Enabled = true;
                    button_stop.Enabled = false;
                    etw = new ETW_Collector();
                    etwlog = new ETW_Logger();
                }
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Consolelog.csv", log2.Text);
                etwlog.fileIOLog.WriteXml("log\\log_allfiles.xml");
                etwlog.fileIOLog.WriteXml("log\\log_malicious.xml");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void ETW_Main_Load(object sender, EventArgs e)
        {
            etwlog = new ETW_Logger();
            ETW_Global.benignProcessNames.Add("System");
            ETW_Global.benignProcessNames.Add("explorer");
            ETW_Global.benignProcessNames.Add("MsMpEng");
            ETW_Global.benignProcessNames.Add("SearchProtocolHost");
            ETW_Global.benignProcessNames.Add("MyFileIOMonitor");
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
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Fonts".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Servicing".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\prefetch".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\assembly".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86).ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\ServiceProfiles".ToLower());
            ETW_Global.excludedDirs.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\ServiceState".ToLower());
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
            textBox_msg.Text = etwlog.fileIOLog.Rows.Count.ToString() + ":" + etwlog.maliciousLogs.Rows.Count.ToString();
            try
            {
                etwlog.fileIOLog.WriteXml("log\\log_allfiles.xml");
                etwlog.maliciousLogs.WriteXml("log\\log_malicious.xml");
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\Consolelog.txt", log2.Text);
                etwlog.AppendConsole2(log2,"Infected Files: " + etwlog.infectedfilescounter.ToString());
                System.IO.File.WriteAllText(ETW_Global.baseDir + "log\\infectedfiles.txt", etwlog.infectedfilenames.toString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkBox_verbose_CheckedChanged(object sender, EventArgs e)
        {
            ETW_Global.VERBOSE1 = checkBox_verbose.Checked;
        }

        private void button_exploreDir_Click(object sender, EventArgs e)
        {
            OpenFolder(ETW_Global.baseDir + "log");
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
            try
            {
                textBox_msg.Text = etwlog.fileIOLog.Rows.Count.ToString() + ":" + etwlog.maliciousLogs.Rows.Count.ToString();               
            }
            catch (Exception ex)
            {
                etwlog.AppendConsole2(log2, "Error getting counter stats: " + ex.Message);
            }
            deleteOldRecords();
            getperformanceCounters();
        }

        private void getperformanceCounters()
        {
            try
            {
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
                PerformanceCounter cpuCounter;
                cpuCounter = new PerformanceCounter("Process", "% Processor Time", "MyFileIOMonitor", true);
                string cpu = cpuCounter.NextValue() + "%";
                this.Text = "Ransomguard_v2.1" + " , " + cpu + " , " + Math.Round((double)totalBytesOfMemoryUsed / 1024 / 1024, 2).ToString() + "MB";

            }
            catch (Exception ex)
            {
                etwlog.AppendConsole2(log2, "Error getting performance counters: " + ex.Message);
            }
        }
        private void deleteOldRecords()
        {
            try
            {
                foreach (DataRow row in etwlog.fileIOLog.Rows)
                {
                    long diff = DateTime.Now.Ticks - (Int64)row["ts"];
                    if (diff > 600000000) { row.Delete(); }
                }
                etwlog.fileIOLog.AcceptChanges();
            }
            catch (Exception ex)
            {
                etwlog.AppendConsole2(log2, "Error while clearing old events: "+ex.Message);
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

        }

        private void ETW_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void checkBox_csv_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void label_analyze_Click(object sender, EventArgs e)
        {

        }
        private void label_save_Click(object sender, EventArgs e)
        {

        }

        private void button_test_Click(object sender, EventArgs e)
        {
            // Create a new file
            FileInfo[] fi = GetFiles("log");
            string ss = "CreatedFile";
            string path = "log" + "\\" + ss + (fi.Count() + 1).ToString() + ".txt";
            using (FileStream fs = File.Create(path))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes("New-Text-File");
                fs.Write(title, 0, title.Length);
                etwlog.AppendConsole2(log2, "File Created");
            }

            try
            {
                using (StreamWriter sw = File.AppendText(Directory.GetFiles("log")[0]))
                {
                    sw.WriteLine("This");
                    etwlog.AppendConsole2(log2, "File modified");
                }
            }
            catch (Exception) { }

            try
            {
                FileInfo file = GetFiles("log")[0];
                string newExtension = ".ext" + DateTime.Now.Minute.ToString();
                string newfilefullname = (file.FullName).Replace(file.Extension, newExtension);
                System.IO.File.Move(file.FullName, newfilefullname);
                etwlog.AppendConsole2(log2, "File Renamed");
            }
            catch { }
        }
        public FileInfo[] GetFiles(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles(); //Getting Txt files
            return Files;
        }

        private void button_csv_Click(object sender, EventArgs e)
        {
            xmltocsv.convert("allfiles", "log\\log_allfiles.xml", "log\\log_allfiles.csv");
            xmltocsv.convert("maliciouslog", "log\\log_malicious.xml", "log\\log_malicious.csv");
            etwlog.AppendConsole2(log2, "CSV Files Saved Successfully");
        }
    }
}

using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Session;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace MyFileIOMonitor
{
    public partial class ETW_Monitor : Form
    {
        ETW_Collector etw;
        ETW_Logger etwlog;
        TraceEventSession KernelSession;
        ETWTraceEventSource source;
        Thread t;
        public ETW_Monitor()
        {
            InitializeComponent();
            //hideCloseButton();
        }

        public bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ETW_Global.monitored_Dir = textBox_monitoredDir.Text;
            toolStripMsg.Text = ETW_Global.monitored_Dir;
            ETW_Global.file1 = true;
            etwlog = new ETW_Logger();           
        }

        private void hideCloseButton()
        {
            [DllImport("user32")]
            static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
            [DllImport("user32")]
            static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
            const int MF_BYCOMMAND = 0;
            const int MF_DISABLED = 2;
            const int SC_CLOSE = 0xF060;
            var sm = GetSystemMenu(Handle, false);
            EnableMenuItem(sm, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED);
        }

        public FileInfo[] GetFiles(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] Files = d.GetFiles(); //Getting Txt files
            return Files;
        }
        private void button_renameFile_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo file = GetFiles(ETW_Global.monitored_Dir)[0];
                string newExtension = ".txt" + DateTime.Now.Minute.ToString();
                string newfilefullname = (file.FullName).Replace(file.Extension, newExtension);
                System.IO.File.Move(file.FullName, newfilefullname);
                etwlog.AppendConsole2(log2, "File Renamed");
            }
            catch { }
        }
        private void button_dbglist_Click(object sender, EventArgs e)
        {
            //events.viewlist();
        }
        private void button_clr_Click(object sender, EventArgs e)
        {
            log.Clear();
            log2.Clear();
            System.GC.Collect();
        }
        private void button_delFiles_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo f = GetFiles(ETW_Global.monitored_Dir)[0];
                System.IO.File.Delete(f.FullName);
                etwlog.AppendConsole2(log2, "File  '" + f.FullName + "' Deleted Successfully.");
            }
            catch { }
        }
        private void button_Browse_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox_monitoredDir.Text = folderBrowserDialog1.SelectedPath;
            }
            etwlog.AppendConsole2(log2, "Selected Directory for monitoring Events : " + folderBrowserDialog1.SelectedPath);
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        { }
        private void button_listfiles_Click(object sender, EventArgs e)
        {
            etwlog.AppendConsole2(log2, "All Directories: ");
            foreach (string f in Directory.GetFiles(ETW_Global.monitored_Dir)) { etwlog.AppendConsole2(log2, f); }
            etwlog.AppendConsole2(log2, "Excluded Directories: ");
            foreach (string s in ETW_Global.excludedDirs) { etwlog.AppendConsole2(log2, s); }
        }
        private void button_createFiles_Click(object sender, EventArgs e)
        {
            // Create a new file
            FileInfo[] fi = GetFiles(ETW_Global.monitored_Dir);
            string ss = "CreatedFile";
            string path = ETW_Global.monitored_Dir + "\\" + ss + (fi.Count() + 1).ToString() + ".txt";
            using (FileStream fs = File.Create(path))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes("New-Text-File");
                fs.Write(title, 0, title.Length);
                etwlog.AppendConsole2(log2, "File Created");
            }
        }
        private void button_ModifyFiles_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(Directory.GetFiles(ETW_Global.monitored_Dir)[0]))
                {
                    sw.WriteLine("This");
                    etwlog.AppendConsole2(log2, "File modified");
                }
            }
            catch (Exception) { }
        }
        private void button_SaveAS_Click(object sender, EventArgs e)
        {
            // log.SaveFile(monitored_Dir + "\\log.txt",RichTextBoxStreamType.PlainText);
            //MessageBox.Show("Events saved to log file successfully");
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            if (checkBox_Filters.Checked) { ETW_Global.EVENT_FILTERS = ETW_Global.FILTER_ALL; }
            else { ETW_Global.EVENT_FILTERS = ETW_Global.FILTER_NONE; }
            string sessionName = KernelTraceEventParser.KernelSessionName;
            KernelSession = new TraceEventSession("MyTraceEventSession", ETW_Global.etlFilePath);
            KernelSession.StopOnDispose = true;
            if (ETW_Global.network1 == false && ETW_Global.file1 == true)
            {
                KernelSession.EnableKernelProvider(
                     KernelTraceEventParser.Keywords.FileIOInit      //required reade/write/create/delete operations
                   | KernelTraceEventParser.Keywords.DiskFileIO    //required for rename operation
                                                                   //| KernelTraceEventParser.Keywords.NetworkTCPIP
                                                                   //| KernelTraceEventParser.Keywords.Registry
                   | KernelTraceEventParser.Keywords.Thread
                   | KernelTraceEventParser.Keywords.Process
                    );
                //MessageBox.Show("f");
            }
            if (ETW_Global.network1 == true && ETW_Global.file1 == false)
            {
                KernelSession.EnableProvider("988C59C5-0A1C-45B6-A555-0C62276E327D");
                //MessageBox.Show("n");
            }
            if (ETW_Global.network1 == true && ETW_Global.file1 == true)
            {
                KernelSession.EnableKernelProvider(
                     KernelTraceEventParser.Keywords.FileIOInit      //required reade/write/create/delete operations
                   | KernelTraceEventParser.Keywords.DiskFileIO    //required for rename operation
                   | KernelTraceEventParser.Keywords.NetworkTCPIP
                    );
                //MessageBox.Show("fn");
            }
            if (ETW_Global.network1 == false && ETW_Global.file1 == false)
            {
                MessageBox.Show("No Provider Selected.");
            }
            button_start.Enabled = false;
            checkBox_Filters.Enabled = false;
            button_stop.Enabled = true;
        }

        //private void StartETW()
        //{
        //    if (!(TraceEventSession.IsElevated() ?? false))
        //    {
        //        etwlog.AppendConsole2(log2, "To turn on ETW events you need to be Administrator, please run from an Admin process.");
        //    }
        //    else
        //    {
        //        etw = new ETW_Collector(log, log2, etwlog, KernelSession);
        //        t = new Thread(() => etw.Run());
        //        t.Start();
        //        button_start.Enabled = false;
        //        checkBox_Filters.Enabled = false;
        //        button_stop.Enabled = true;
        //    }
        //}
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form_SelectDirectories frm = new Form_SelectDirectories();
            frm.Show();
        }

        private void buttonViewList_Click(object sender, EventArgs e)
        {
            etwlog.AppendConsole2(log2, "ParentThreadIDMapping List");
            foreach (var item in etwlog.ParentThreadIDMapping)
            {
                etwlog.AppendConsole2(log2, "ThreadID : " + item.Key.ToString() + " ParentThread : " + item.Value.ToString());
            }
            etwlog.AppendConsole2(log2, "ParentProcessIDMapping List");
            foreach (var item in etwlog.ParentProcessIDMapping)
            {
                etwlog.AppendConsole2(log2, "ProcessID : " + item.Key.ToString() + " ParentProcessID : " + item.Value.ToString());
            }
            etwlog.AppendConsole2(log2, "Process List");
            foreach (var item in etwlog.ProcessList)
            {
                etwlog.AppendConsole2(log2, "ProcessID : " + item.Key.ToString() + " ProcessName : " + item.Value.ToString());
            }
            etwlog.AppendConsole2(log2, "Files List");
            foreach (var item in etwlog.FileList)
            {
                etwlog.AppendConsole2(log2, "FileKey : " + item.Key.ToString() + " FileName : " + item.Value.ToString());
            }
        }

        private void button_hide_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {

        }

        private void ETW_Monitor_VisibleChanged(object sender, EventArgs e)
        {
            //hideCloseButton();
        }

        private void ETW_Monitor_Resize(object sender, EventArgs e)
        {
            //hideCloseButton();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void button_ParseLog_Click(object sender, EventArgs e)
        {
            ETW_Parser parser = new ETW_Parser(log, log2, etwlog);
            parser.parseAllLogFiles();
        }

        private void checkBox_verbose_CheckedChanged(object sender, EventArgs e)
        {
            ETW_Global.VERBOSE1 = checkBox_verbose.Checked;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button_parse_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                button_stopParsing.Enabled = true;
                toolStripMsg.Text = openFileDialog1.FileName;
                etw = new ETW_Collector(log, log2, etwlog);
                source = new ETWTraceEventSource(openFileDialog1.FileName);
                source.Kernel.All += etw.HandleCallBacks;
                t = new Thread(() => etw.ProcessEtl(source, toolStripStatus2));
                t.Start();
            }
        }

        private void button_stopParsing_Click(object sender, EventArgs e)
        {
            if (t.IsAlive)
            {
                source.StopProcessing();
                Thread.Sleep(1000);
            }
            if (!t.IsAlive)
            {
                etwlog.savelogFiles();
                etwlog.AppendConsole2(log2, "Events saved to log file successfully");
                toolStripStatus2.Text = "Events saved to log file successfully";
                etwlog.AppendConsole2(log2, "********  Stop processing Kernal Events  *******");
                //UncheckAllItems();
                button_start.Enabled = true;
                checkBox_Filters.Enabled = true;
                button_stop.Enabled = false;
                button_stopParsing.Enabled = false;
                //checkedListBox1.Enabled = true;
                etw = new ETW_Collector();
                etwlog = new ETW_Logger();
            }
        }

        private void buttonhalt_Click(object sender, EventArgs e)
        {
            if (t.IsAlive)
            {
                source.StopProcessing();
                Thread.Sleep(1000);
            }
            if (!t.IsAlive)
            {
                etwlog.AppendConsole2(log2, "********  Halt processing Kernal Events  *******");
                //UncheckAllItems();
                button_start.Enabled = true;
                checkBox_Filters.Enabled = true;
                button_stop.Enabled = false;
                //checkedListBox1.Enabled = true;
                etw = new ETW_Collector();
                etwlog = new ETW_Logger();
            }
        }

        private void textBox_monitoredDir_TextChanged(object sender, EventArgs e)
        {
            ETW_Global.monitored_Dir = textBox_monitoredDir.Text;
        }

        private void checkBox_Filters_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_FileIO_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox_NetworkIO_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox_verboselog2_CheckedChanged(object sender, EventArgs e)
        {
            ETW_Global.VERBOSE2 = checkBox_verboselog2.Checked;
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            KernelSession.Dispose();
            Thread.Sleep(1000);
            button_start.Enabled = true;
            checkBox_Filters.Enabled = true;
            button_stop.Enabled = false;
        }
        public void resetETW()
        {
            try
            {
                if (t.IsAlive)
                {
                    KernelSession.Dispose();
                    Thread.Sleep(1000);
                    //etw.StopETW(); 
                }
                if (!t.IsAlive)
                {
                    etwlog.savelogFiles();
                    etwlog.AppendConsole2(log2, "Events saved to log file successfully");
                    etwlog.AppendConsole2(log2, "********  Stop processing Kernal Events  *******");
                    //UncheckAllItems();
                    button_start.Enabled = true;
                    checkBox_Filters.Enabled = true;
                    button_stop.Enabled = false;
                    //checkedListBox1.Enabled = true;
                    etw = new ETW_Collector();
                    etwlog = new ETW_Logger();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //private void UncheckAllItems()
        //{
        //    checkedListBox1.ClearSelected();
        //    while (checkedListBox1.CheckedIndices.Count > 0)
        //        checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
        //}
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (checkedListBox1.CheckedIndices.Contains(0))
            //{
            //    file = true;
            //}
            //else { file = false; }
            //if (checkedListBox1.CheckedIndices.Contains(1))
            //{
            //    process = true;
            //}
            //else { process = false; }
            //if (checkedListBox1.CheckedIndices.Contains(2))
            //{
            //    thread = true;
            //}
            //else { thread = false; }
            //if (checkedListBox1.CheckedIndices.Contains(3))
            //{
            //    reg = true;
            //}
            //else { reg = false; }
            //if (checkedListBox1.CheckedIndices.Contains(4))
            //{
            //    network = true;
            //}
            //else { network = false; }
            //if (checkedListBox1.CheckedIndices.Contains(5))
            //{
            //    all = true;
            //}
            //else { all = false; }
        }

        private void buttonPyScript_Click(object sender, EventArgs e)
        {
            string scriptPath = @"d:\My_Repositories\ETW_Processor\inference.pyw";
            string python = @"C:\Users\alvi\AppData\Local\Programs\Python\Python39\pythonw.exe";
            string path = @"C:\Users\alvi\Downloads\";
            string csvFilename = @"1c-benign.csv";
            string modelName = @"model_rf.pkl";
            string[] str = run_script(scriptPath, python, path, csvFilename, modelName);
            MessageBox.Show(str[0] + " " + str[1]);
        }
        private string[] run_script(string scriptPath, string python, string path, string csvFilename, string modelName)
        {
            Process psi = new Process();
            string args = scriptPath + " " + path + " " + csvFilename + " " + modelName;
            psi.StartInfo = new ProcessStartInfo(python, args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };
            psi.Start();
            string output = psi.StandardOutput.ReadToEnd();
            psi.WaitForExit();
            string maxper = (output.Split(' ')[0]).Split('=')[1];
            string mpid = (output.Split(' ')[1]).Split('=')[1];
            string[] arr = { mpid.Trim(), maxper.Trim() };
            return arr;
        }
    }
}
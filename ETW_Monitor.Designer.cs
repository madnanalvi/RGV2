namespace MyFileIOMonitor
{
    partial class ETW_Monitor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ETW_Monitor));
            log = new RichTextBox();
            button_renameFile = new Button();
            button_clr = new Button();
            button_delFiles = new Button();
            button_createFiles = new Button();
            button_ModifyFiles = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button_Browse = new Button();
            button_listfiles = new Button();
            log2 = new RichTextBox();
            statusStrip1 = new StatusStrip();
            toolStripMsg = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatus2 = new ToolStripStatusLabel();
            button_start = new Button();
            button_stop = new Button();
            bindingSource1 = new BindingSource(components);
            button_exploreDir = new Button();
            button1 = new Button();
            buttonViewList = new Button();
            button_hide = new Button();
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            showToolStripMenuItem = new ToolStripMenuItem();
            quitToolStripMenuItem = new ToolStripMenuItem();
            checkBox_Filters = new CheckBox();
            checkBox_verbose = new CheckBox();
            checkBox_verboselog2 = new CheckBox();
            button_parse = new Button();
            openFileDialog1 = new OpenFileDialog();
            button_stopParsing = new Button();
            label1 = new Label();
            label2 = new Label();
            buttonhalt = new Button();
            textBox_monitoredDir = new TextBox();
            buttonPyScript = new Button();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // log
            // 
            log.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            log.Location = new Point(251, 55);
            log.Margin = new Padding(3, 4, 3, 4);
            log.Name = "log";
            log.ReadOnly = true;
            log.Size = new Size(942, 377);
            log.TabIndex = 0;
            log.Text = "";
            // 
            // button_renameFile
            // 
            button_renameFile.Location = new Point(485, 12);
            button_renameFile.Margin = new Padding(3, 4, 3, 4);
            button_renameFile.Name = "button_renameFile";
            button_renameFile.Size = new Size(67, 31);
            button_renameFile.TabIndex = 1;
            button_renameFile.Text = "Rename";
            button_renameFile.UseVisualStyleBackColor = true;
            button_renameFile.Click += button_renameFile_Click;
            // 
            // button_clr
            // 
            button_clr.Location = new Point(7, 369);
            button_clr.Margin = new Padding(3, 4, 3, 4);
            button_clr.Name = "button_clr";
            button_clr.Size = new Size(70, 31);
            button_clr.TabIndex = 4;
            button_clr.Text = "Clear";
            button_clr.UseVisualStyleBackColor = true;
            button_clr.Click += button_clr_Click;
            // 
            // button_delFiles
            // 
            button_delFiles.Location = new Point(559, 12);
            button_delFiles.Margin = new Padding(3, 4, 3, 4);
            button_delFiles.Name = "button_delFiles";
            button_delFiles.Size = new Size(85, 31);
            button_delFiles.TabIndex = 5;
            button_delFiles.Text = "Delete File";
            button_delFiles.UseVisualStyleBackColor = true;
            button_delFiles.Click += button_delFiles_Click;
            // 
            // button_createFiles
            // 
            button_createFiles.Location = new Point(650, 12);
            button_createFiles.Margin = new Padding(3, 4, 3, 4);
            button_createFiles.Name = "button_createFiles";
            button_createFiles.Size = new Size(109, 31);
            button_createFiles.TabIndex = 6;
            button_createFiles.Text = "Copy/Create";
            button_createFiles.UseVisualStyleBackColor = true;
            button_createFiles.Click += button_createFiles_Click;
            // 
            // button_ModifyFiles
            // 
            button_ModifyFiles.Location = new Point(766, 12);
            button_ModifyFiles.Margin = new Padding(3, 4, 3, 4);
            button_ModifyFiles.Name = "button_ModifyFiles";
            button_ModifyFiles.Size = new Size(91, 31);
            button_ModifyFiles.TabIndex = 7;
            button_ModifyFiles.Text = "Modify File";
            button_ModifyFiles.UseVisualStyleBackColor = true;
            button_ModifyFiles.Click += button_ModifyFiles_Click;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.SelectedPath = "C:\\";
            folderBrowserDialog1.HelpRequest += folderBrowserDialog1_HelpRequest;
            // 
            // button_Browse
            // 
            button_Browse.Location = new Point(1041, 12);
            button_Browse.Margin = new Padding(3, 4, 3, 4);
            button_Browse.Name = "button_Browse";
            button_Browse.Size = new Size(79, 31);
            button_Browse.TabIndex = 8;
            button_Browse.Text = "Decoy Dir";
            button_Browse.UseVisualStyleBackColor = true;
            button_Browse.Click += button_Browse_Click;
            // 
            // button_listfiles
            // 
            button_listfiles.Location = new Point(199, 15);
            button_listfiles.Margin = new Padding(3, 4, 3, 4);
            button_listfiles.Name = "button_listfiles";
            button_listfiles.Size = new Size(70, 31);
            button_listfiles.TabIndex = 9;
            button_listfiles.Text = "List";
            button_listfiles.UseVisualStyleBackColor = true;
            button_listfiles.Click += button_listfiles_Click;
            // 
            // log2
            // 
            log2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            log2.Location = new Point(5, 441);
            log2.Margin = new Padding(3, 4, 3, 4);
            log2.Name = "log2";
            log2.ReadOnly = true;
            log2.Size = new Size(1189, 281);
            log2.TabIndex = 10;
            log2.Text = "";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripMsg, toolStripStatusLabel1, toolStripStatus2 });
            statusStrip1.Location = new Point(0, 731);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(1208, 26);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripMsg
            // 
            toolStripMsg.Name = "toolStripMsg";
            toolStripMsg.Size = new Size(15, 20);
            toolStripMsg.Text = "-";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Enabled = false;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(112, 20);
            toolStripStatusLabel1.Text = "Copyrights: Alvi";
            toolStripStatusLabel1.Click += toolStripStatusLabel1_Click;
            // 
            // toolStripStatus2
            // 
            toolStripStatus2.Name = "toolStripStatus2";
            toolStripStatus2.Size = new Size(15, 20);
            toolStripStatus2.Text = "-";
            // 
            // button_start
            // 
            button_start.Location = new Point(7, 82);
            button_start.Margin = new Padding(3, 4, 3, 4);
            button_start.Name = "button_start";
            button_start.Size = new Size(70, 31);
            button_start.TabIndex = 13;
            button_start.Text = "start";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // button_stop
            // 
            button_stop.Location = new Point(83, 82);
            button_stop.Margin = new Padding(3, 4, 3, 4);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(70, 31);
            button_stop.TabIndex = 14;
            button_stop.Text = "stop";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // button_exploreDir
            // 
            button_exploreDir.Location = new Point(5, 12);
            button_exploreDir.Margin = new Padding(3, 4, 3, 4);
            button_exploreDir.Name = "button_exploreDir";
            button_exploreDir.Size = new Size(79, 31);
            button_exploreDir.TabIndex = 16;
            button_exploreDir.Text = "Open Dir";
            button_exploreDir.UseVisualStyleBackColor = true;
            button_exploreDir.Click += button_exploreDir_Click;
            // 
            // button1
            // 
            button1.Location = new Point(90, 15);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(102, 31);
            button1.TabIndex = 20;
            button1.Text = "Excluded Dirs";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // buttonViewList
            // 
            buttonViewList.Location = new Point(83, 369);
            buttonViewList.Margin = new Padding(3, 4, 3, 4);
            buttonViewList.Name = "buttonViewList";
            buttonViewList.Size = new Size(70, 31);
            buttonViewList.TabIndex = 21;
            buttonViewList.Text = "ViewList";
            buttonViewList.UseVisualStyleBackColor = true;
            buttonViewList.Click += buttonViewList_Click;
            // 
            // button_hide
            // 
            button_hide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_hide.Location = new Point(1127, 11);
            button_hide.Margin = new Padding(3, 4, 3, 4);
            button_hide.Name = "button_hide";
            button_hide.Size = new Size(67, 31);
            button_hide.TabIndex = 22;
            button_hide.Text = "Hide";
            button_hide.UseVisualStyleBackColor = true;
            button_hide.Click += button_hide_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "ETW Monitor";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { showToolStripMenuItem, quitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(115, 52);
            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new Size(114, 24);
            showToolStripMenuItem.Text = "&Show";
            showToolStripMenuItem.Click += showToolStripMenuItem_Click;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new Size(114, 24);
            quitToolStripMenuItem.Text = "&Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // checkBox_Filters
            // 
            checkBox_Filters.AutoSize = true;
            checkBox_Filters.Location = new Point(275, 17);
            checkBox_Filters.Margin = new Padding(3, 4, 3, 4);
            checkBox_Filters.Name = "checkBox_Filters";
            checkBox_Filters.Size = new Size(113, 24);
            checkBox_Filters.TabIndex = 23;
            checkBox_Filters.Text = "Apply Filters";
            checkBox_Filters.UseVisualStyleBackColor = true;
            checkBox_Filters.CheckedChanged += checkBox_Filters_CheckedChanged;
            // 
            // checkBox_verbose
            // 
            checkBox_verbose.AutoSize = true;
            checkBox_verbose.Location = new Point(384, 17);
            checkBox_verbose.Margin = new Padding(3, 4, 3, 4);
            checkBox_verbose.Name = "checkBox_verbose";
            checkBox_verbose.Size = new Size(84, 24);
            checkBox_verbose.TabIndex = 25;
            checkBox_verbose.Text = "Verbose";
            checkBox_verbose.UseVisualStyleBackColor = true;
            checkBox_verbose.CheckedChanged += checkBox_verbose_CheckedChanged;
            // 
            // checkBox_verboselog2
            // 
            checkBox_verboselog2.AutoSize = true;
            checkBox_verboselog2.Checked = true;
            checkBox_verboselog2.CheckState = CheckState.Checked;
            checkBox_verboselog2.Location = new Point(7, 408);
            checkBox_verboselog2.Margin = new Padding(3, 4, 3, 4);
            checkBox_verboselog2.Name = "checkBox_verboselog2";
            checkBox_verboselog2.Size = new Size(84, 24);
            checkBox_verboselog2.TabIndex = 26;
            checkBox_verboselog2.Text = "Verbose";
            checkBox_verboselog2.UseVisualStyleBackColor = true;
            checkBox_verboselog2.CheckedChanged += checkBox_verboselog2_CheckedChanged;
            // 
            // button_parse
            // 
            button_parse.Location = new Point(7, 156);
            button_parse.Margin = new Padding(3, 4, 3, 4);
            button_parse.Name = "button_parse";
            button_parse.Size = new Size(70, 31);
            button_parse.TabIndex = 27;
            button_parse.Text = "Parse ETL";
            button_parse.UseVisualStyleBackColor = true;
            button_parse.Click += button_parse_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_stopParsing
            // 
            button_stopParsing.Location = new Point(83, 156);
            button_stopParsing.Margin = new Padding(3, 4, 3, 4);
            button_stopParsing.Name = "button_stopParsing";
            button_stopParsing.Size = new Size(70, 31);
            button_stopParsing.TabIndex = 28;
            button_stopParsing.Text = "Save Log";
            button_stopParsing.UseVisualStyleBackColor = true;
            button_stopParsing.Click += button_stopParsing_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 58);
            label1.Name = "label1";
            label1.Size = new Size(61, 20);
            label1.TabIndex = 29;
            label1.Text = "Capture";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 132);
            label2.Name = "label2";
            label2.Size = new Size(56, 20);
            label2.TabIndex = 30;
            label2.Text = "Parsing";
            // 
            // buttonhalt
            // 
            buttonhalt.Location = new Point(160, 156);
            buttonhalt.Margin = new Padding(3, 4, 3, 4);
            buttonhalt.Name = "buttonhalt";
            buttonhalt.Size = new Size(70, 31);
            buttonhalt.TabIndex = 31;
            buttonhalt.Text = "Stop";
            buttonhalt.UseVisualStyleBackColor = true;
            buttonhalt.Click += buttonhalt_Click;
            // 
            // textBox_monitoredDir
            // 
            textBox_monitoredDir.Location = new Point(864, 12);
            textBox_monitoredDir.Margin = new Padding(3, 4, 3, 4);
            textBox_monitoredDir.Name = "textBox_monitoredDir";
            textBox_monitoredDir.Size = new Size(170, 27);
            textBox_monitoredDir.TabIndex = 32;
            textBox_monitoredDir.Text = "C:\\Users\\alvi\\Documents";
            textBox_monitoredDir.TextChanged += textBox_monitoredDir_TextChanged;
            // 
            // buttonPyScript
            // 
            buttonPyScript.Location = new Point(7, 330);
            buttonPyScript.Margin = new Padding(3, 4, 3, 4);
            buttonPyScript.Name = "buttonPyScript";
            buttonPyScript.Size = new Size(102, 31);
            buttonPyScript.TabIndex = 36;
            buttonPyScript.Text = "execScript";
            buttonPyScript.UseVisualStyleBackColor = true;
            buttonPyScript.Click += buttonPyScript_Click;
            // 
            // ETW_Monitor
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1208, 757);
            Controls.Add(buttonPyScript);
            Controls.Add(textBox_monitoredDir);
            Controls.Add(buttonhalt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button_stopParsing);
            Controls.Add(button_parse);
            Controls.Add(checkBox_verboselog2);
            Controls.Add(checkBox_verbose);
            Controls.Add(checkBox_Filters);
            Controls.Add(button_hide);
            Controls.Add(buttonViewList);
            Controls.Add(button1);
            Controls.Add(button_exploreDir);
            Controls.Add(button_stop);
            Controls.Add(button_start);
            Controls.Add(statusStrip1);
            Controls.Add(log2);
            Controls.Add(button_listfiles);
            Controls.Add(button_Browse);
            Controls.Add(button_ModifyFiles);
            Controls.Add(button_createFiles);
            Controls.Add(button_delFiles);
            Controls.Add(button_clr);
            Controls.Add(button_renameFile);
            Controls.Add(log);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "ETW_Monitor";
            Text = "ETW Monitor";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            VisibleChanged += ETW_Monitor_VisibleChanged;
            Resize += ETW_Monitor_Resize;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox log;
        private Button button_renameFile;
        private Button button_clr;
        private Button button_delFiles;
        private Button button_createFiles;
        private Button button_ModifyFiles;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button_Browse;
        private Button button_listfiles;
        private RichTextBox log2;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripMsg;
        private Button button_start;
        private Button button_stop;
        private BindingSource bindingSource1;
        private Button button_exploreDir;
        private Button button1;
        private Button buttonViewList;
        private Button button_hide;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private CheckBox checkBox_Filters;
        private CheckBox checkBox_verbose;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private CheckBox checkBox_verboselog2;
        private Button button_parse;
        private OpenFileDialog openFileDialog1;
        private Button button_stopParsing;
        private Label label1;
        private Label label2;
        private ToolStripStatusLabel toolStripStatus2;
        private Button buttonhalt;
        private TextBox textBox_monitoredDir;
        private Button buttonPyScript;
    }
}
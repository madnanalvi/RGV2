namespace MyFileIOMonitor
{
    partial class ETW_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ETW_Main));
            button_stop = new Button();
            button_start = new Button();
            log = new RichTextBox();
            buttonPyScript = new Button();
            checkBox_verbose = new CheckBox();
            button_exploreDir = new Button();
            button_clr = new Button();
            checkBox_verbose2 = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            button_hide = new Button();
            notifyIcon1 = new NotifyIcon(components);
            label1 = new Label();
            label2 = new Label();
            log2 = new RichTextBox();
            splitContainer1 = new SplitContainer();
            checkBox_debug = new CheckBox();
            label_msg = new Label();
            textBox_msg = new TextBox();
            checkBox_csv = new CheckBox();
            label_analyze = new Label();
            openFileDialog1 = new OpenFileDialog();
            label_save = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // button_stop
            // 
            button_stop.Enabled = false;
            button_stop.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button_stop.ForeColor = Color.Maroon;
            button_stop.Location = new Point(77, 4);
            button_stop.Name = "button_stop";
            button_stop.Size = new Size(61, 23);
            button_stop.TabIndex = 31;
            button_stop.Text = "STOP";
            button_stop.UseVisualStyleBackColor = true;
            button_stop.Click += button_stop_Click;
            // 
            // button_start
            // 
            button_start.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button_start.ForeColor = Color.DarkCyan;
            button_start.Location = new Point(10, 4);
            button_start.Name = "button_start";
            button_start.Size = new Size(61, 23);
            button_start.TabIndex = 30;
            button_start.Text = "START";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += button_start_Click;
            // 
            // log
            // 
            log.BackColor = SystemColors.Desktop;
            log.BorderStyle = BorderStyle.None;
            log.Dock = DockStyle.Fill;
            log.ForeColor = SystemColors.Window;
            log.Location = new Point(0, 0);
            log.Name = "log";
            log.ReadOnly = true;
            log.Size = new Size(354, 77);
            log.TabIndex = 33;
            log.Text = "";
            // 
            // buttonPyScript
            // 
            buttonPyScript.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPyScript.Location = new Point(297, 30);
            buttonPyScript.Name = "buttonPyScript";
            buttonPyScript.Size = new Size(68, 23);
            buttonPyScript.TabIndex = 37;
            buttonPyScript.Text = "viewlog";
            buttonPyScript.UseVisualStyleBackColor = true;
            buttonPyScript.Click += buttonPyScript_Click;
            // 
            // checkBox_verbose
            // 
            checkBox_verbose.AutoSize = true;
            checkBox_verbose.Location = new Point(10, 33);
            checkBox_verbose.Name = "checkBox_verbose";
            checkBox_verbose.Size = new Size(73, 19);
            checkBox_verbose.TabIndex = 38;
            checkBox_verbose.Text = "Verbose1";
            checkBox_verbose.UseVisualStyleBackColor = true;
            checkBox_verbose.CheckedChanged += checkBox_verbose_CheckedChanged;
            // 
            // button_exploreDir
            // 
            button_exploreDir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_exploreDir.Location = new Point(232, 4);
            button_exploreDir.Name = "button_exploreDir";
            button_exploreDir.Size = new Size(69, 23);
            button_exploreDir.TabIndex = 39;
            button_exploreDir.Text = "Open Dir";
            button_exploreDir.UseVisualStyleBackColor = true;
            button_exploreDir.Click += button_exploreDir_Click;
            // 
            // button_clr
            // 
            button_clr.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_clr.Location = new Point(165, 4);
            button_clr.Name = "button_clr";
            button_clr.Size = new Size(61, 23);
            button_clr.TabIndex = 40;
            button_clr.Text = "Clear";
            button_clr.UseVisualStyleBackColor = true;
            button_clr.Click += button_clr_Click;
            // 
            // checkBox_verbose2
            // 
            checkBox_verbose2.AutoSize = true;
            checkBox_verbose2.Checked = true;
            checkBox_verbose2.CheckState = CheckState.Checked;
            checkBox_verbose2.Location = new Point(96, 33);
            checkBox_verbose2.Name = "checkBox_verbose2";
            checkBox_verbose2.Size = new Size(73, 19);
            checkBox_verbose2.TabIndex = 41;
            checkBox_verbose2.Text = "Verbose2";
            checkBox_verbose2.UseVisualStyleBackColor = true;
            checkBox_verbose2.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10000;
            timer1.Tick += timer1_Tick;
            // 
            // button_hide
            // 
            button_hide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_hide.Location = new Point(306, 4);
            button_hide.Name = "button_hide";
            button_hide.Size = new Size(59, 23);
            button_hide.TabIndex = 42;
            button_hide.Text = "Hide";
            button_hide.UseVisualStyleBackColor = true;
            button_hide.Click += button_hide_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Enabled = false;
            label1.ForeColor = SystemColors.ControlDarkDark;
            label1.Location = new Point(242, 218);
            label1.Name = "label1";
            label1.Size = new Size(116, 15);
            label1.TabIndex = 43;
            label1.Text = "Copyrights.. MA.Alvi";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(10, 218);
            label2.Name = "label2";
            label2.Size = new Size(25, 15);
            label2.TabIndex = 44;
            label2.Text = "ETL";
            label2.Click += label2_Click;
            // 
            // log2
            // 
            log2.BackColor = SystemColors.Desktop;
            log2.BorderStyle = BorderStyle.None;
            log2.Dock = DockStyle.Fill;
            log2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            log2.ForeColor = SystemColors.Window;
            log2.Location = new Point(0, 0);
            log2.Name = "log2";
            log2.ReadOnly = true;
            log2.Size = new Size(354, 77);
            log2.TabIndex = 34;
            log2.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(10, 58);
            splitContainer1.Margin = new Padding(3, 2, 3, 2);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(log);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(log2);
            splitContainer1.Size = new Size(354, 157);
            splitContainer1.SplitterDistance = 77;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 45;
            // 
            // checkBox_debug
            // 
            checkBox_debug.AutoSize = true;
            checkBox_debug.Location = new Point(182, 33);
            checkBox_debug.Name = "checkBox_debug";
            checkBox_debug.Size = new Size(60, 19);
            checkBox_debug.TabIndex = 46;
            checkBox_debug.Text = "debug";
            checkBox_debug.UseVisualStyleBackColor = true;
            checkBox_debug.CheckedChanged += checkBox_debug_CheckedChanged;
            // 
            // label_msg
            // 
            label_msg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_msg.AutoSize = true;
            label_msg.Enabled = false;
            label_msg.ForeColor = SystemColors.ControlDarkDark;
            label_msg.Location = new Point(41, 217);
            label_msg.Name = "label_msg";
            label_msg.Size = new Size(12, 15);
            label_msg.TabIndex = 47;
            label_msg.Text = "-";
            // 
            // textBox_msg
            // 
            textBox_msg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox_msg.BorderStyle = BorderStyle.None;
            textBox_msg.Location = new Point(242, 34);
            textBox_msg.Name = "textBox_msg";
            textBox_msg.Size = new Size(49, 16);
            textBox_msg.TabIndex = 48;
            // 
            // checkBox_csv
            // 
            checkBox_csv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            checkBox_csv.AutoSize = true;
            checkBox_csv.Checked = true;
            checkBox_csv.CheckState = CheckState.Checked;
            checkBox_csv.Location = new Point(59, 216);
            checkBox_csv.Name = "checkBox_csv";
            checkBox_csv.Size = new Size(86, 19);
            checkBox_csv.TabIndex = 49;
            checkBox_csv.Text = "Export_CSV";
            checkBox_csv.UseVisualStyleBackColor = true;
            checkBox_csv.CheckedChanged += checkBox_csv_CheckedChanged;
            // 
            // label_analyze
            // 
            label_analyze.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_analyze.AutoSize = true;
            label_analyze.Location = new Point(182, 218);
            label_analyze.Name = "label_analyze";
            label_analyze.Size = new Size(48, 15);
            label_analyze.TabIndex = 50;
            label_analyze.Text = "Analyze";
            label_analyze.Click += label_analyze_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label_save
            // 
            label_save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_save.AutoSize = true;
            label_save.Location = new Point(151, 218);
            label_save.Name = "label_save";
            label_save.Size = new Size(31, 15);
            label_save.TabIndex = 51;
            label_save.Text = "Save";
            label_save.Click += label_save_Click;
            // 
            // ETW_Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.GhostWhite;
            ClientSize = new Size(375, 235);
            Controls.Add(label_save);
            Controls.Add(label_analyze);
            Controls.Add(checkBox_csv);
            Controls.Add(textBox_msg);
            Controls.Add(label_msg);
            Controls.Add(checkBox_debug);
            Controls.Add(splitContainer1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button_hide);
            Controls.Add(checkBox_verbose2);
            Controls.Add(button_clr);
            Controls.Add(button_exploreDir);
            Controls.Add(checkBox_verbose);
            Controls.Add(buttonPyScript);
            Controls.Add(button_stop);
            Controls.Add(button_start);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "ETW_Main";
            Text = "RansomGuard v1.4";
            FormClosing += ETW_Main_FormClosing;
            Load += ETW_Main_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button_stop;
        private Button button_start;
        private RichTextBox log;
        private Button buttonPyScript;
        private CheckBox checkBox_verbose;
        private Button button_exploreDir;
        private Button button_clr;
        private CheckBox checkBox_verbose2;
        private System.Windows.Forms.Timer timer1;
        private Button button_hide;
        private NotifyIcon notifyIcon1;
        private Label label1;
        private Label label2;
        private RichTextBox log2;
        private SplitContainer splitContainer1;
        private CheckBox checkBox_debug;
        private Label label_msg;
        private TextBox textBox_msg;
        private CheckBox checkBox_csv;
        private Label label_analyze;
        private OpenFileDialog openFileDialog1;
        private Label label_save;
    }
}
namespace MyFileIOMonitor
{
    partial class ETW_Main2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ETW_Main2));
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
            label_msg = new Label();
            textBox_msg = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            button_test = new Button();
            button_csv = new Button();
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
            log.Size = new Size(448, 96);
            log.TabIndex = 33;
            log.Text = "";
            // 
            // buttonPyScript
            // 
            buttonPyScript.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonPyScript.Location = new Point(390, 30);
            buttonPyScript.Name = "buttonPyScript";
            buttonPyScript.Size = new Size(68, 23);
            buttonPyScript.TabIndex = 37;
            buttonPyScript.Text = "saveLog";
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
            button_exploreDir.Location = new Point(326, 4);
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
            button_clr.Location = new Point(259, 4);
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
            button_hide.Location = new Point(400, 4);
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
            label1.Location = new Point(336, 260);
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
            label2.Location = new Point(10, 260);
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
            log2.Size = new Size(448, 99);
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
            splitContainer1.Size = new Size(448, 198);
            splitContainer1.SplitterDistance = 96;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 45;
            // 
            // label_msg
            // 
            label_msg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label_msg.AutoSize = true;
            label_msg.Enabled = false;
            label_msg.ForeColor = SystemColors.ControlDarkDark;
            label_msg.Location = new Point(41, 258);
            label_msg.Name = "label_msg";
            label_msg.Size = new Size(12, 15);
            label_msg.TabIndex = 47;
            label_msg.Text = "-";
            // 
            // textBox_msg
            // 
            textBox_msg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox_msg.BorderStyle = BorderStyle.None;
            textBox_msg.Location = new Point(326, 34);
            textBox_msg.Name = "textBox_msg";
            textBox_msg.Size = new Size(60, 16);
            textBox_msg.TabIndex = 48;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button_test
            // 
            button_test.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_test.Location = new Point(259, 29);
            button_test.Name = "button_test";
            button_test.Size = new Size(61, 23);
            button_test.TabIndex = 49;
            button_test.Text = "test";
            button_test.UseVisualStyleBackColor = true;
            button_test.Click += button_test_Click;
            // 
            // button_csv
            // 
            button_csv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_csv.Location = new Point(215, 4);
            button_csv.Name = "button_csv";
            button_csv.Size = new Size(38, 23);
            button_csv.TabIndex = 50;
            button_csv.Text = "CSV";
            button_csv.UseVisualStyleBackColor = true;
            button_csv.Click += button_csv_Click;
            // 
            // ETW_Main2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.GhostWhite;
            ClientSize = new Size(469, 276);
            Controls.Add(button_csv);
            Controls.Add(button_test);
            Controls.Add(textBox_msg);
            Controls.Add(label_msg);
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
            Name = "ETW_Main2";
            Text = "RansomGuard V2";
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
        private Label label_msg;
        private TextBox textBox_msg;
        private OpenFileDialog openFileDialog1;
        private Button button_test;
        private Button button_csv;
    }
}
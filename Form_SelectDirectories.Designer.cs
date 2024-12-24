namespace MyFileIOMonitor
{
    partial class Form_SelectDirectories
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
            this.listBox_allDirs = new System.Windows.Forms.ListBox();
            this.listBox_selectedDirs = new System.Windows.Forms.ListBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.button_remove = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_save = new System.Windows.Forms.Button();
            this.button_Browse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // listBox_allDirs
            // 
            this.listBox_allDirs.FormattingEnabled = true;
            this.listBox_allDirs.ItemHeight = 15;
            this.listBox_allDirs.Location = new System.Drawing.Point(18, 43);
            this.listBox_allDirs.Name = "listBox_allDirs";
            this.listBox_allDirs.Size = new System.Drawing.Size(306, 349);
            this.listBox_allDirs.TabIndex = 0;
            // 
            // listBox_selectedDirs
            // 
            this.listBox_selectedDirs.FormattingEnabled = true;
            this.listBox_selectedDirs.ItemHeight = 15;
            this.listBox_selectedDirs.Location = new System.Drawing.Point(435, 43);
            this.listBox_selectedDirs.Name = "listBox_selectedDirs";
            this.listBox_selectedDirs.Size = new System.Drawing.Size(306, 349);
            this.listBox_selectedDirs.TabIndex = 1;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(330, 43);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(99, 23);
            this.button_Add.TabIndex = 2;
            this.button_Add.Text = "ADD >>";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // button_remove
            // 
            this.button_remove.Location = new System.Drawing.Point(330, 89);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(99, 23);
            this.button_remove.TabIndex = 3;
            this.button_remove.Text = "<< REMOVE";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Enabled = false;
            this.textBox_path.Location = new System.Drawing.Point(93, 12);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.ReadOnly = true;
            this.textBox_path.Size = new System.Drawing.Size(648, 23);
            this.textBox_path.TabIndex = 4;
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(330, 158);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(99, 23);
            this.button_save.TabIndex = 5;
            this.button_save.Text = "SAVE";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(18, 12);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(69, 23);
            this.button_Browse.TabIndex = 9;
            this.button_Browse.Text = "Select Dir";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.SelectedPath = "C:\\";
            // 
            // Form_SelectDirectories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 407);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.textBox_path);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.button_Add);
            this.Controls.Add(this.listBox_selectedDirs);
            this.Controls.Add(this.listBox_allDirs);
            this.Name = "Form_SelectDirectories";
            this.Text = "Select Directories";
            this.Load += new System.EventHandler(this.Form_SelectDirectories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox listBox_allDirs;
        private ListBox listBox_selectedDirs;
        private Button button_Add;
        private Button button_remove;
        private TextBox textBox_path;
        private Button button_save;
        private Button button_Browse;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}
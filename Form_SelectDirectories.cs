using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFileIOMonitor
{
    public partial class Form_SelectDirectories : Form
    {
        public Form_SelectDirectories()
        {
            InitializeComponent();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            listBox_selectedDirs.Items.Add(listBox_allDirs.Text.ToLower());
            ETW_Global.excludedDirs.Add(listBox_allDirs.Text.ToLower());
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox_path.Text = folderBrowserDialog1.SelectedPath;
                listBox_allDirs.DataSource = Directory.GetDirectories(textBox_path.Text).ToList();
            }
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            var dir = listBox_selectedDirs.Text.ToLower();
            listBox_selectedDirs.Items.Remove(dir);
            ETW_Global.excludedDirs.Remove(dir);
        }

        private void Form_SelectDirectories_Load(object sender, EventArgs e)
        {
            textBox_path.Text = ETW_Global.monitored_Dir;
            listBox_allDirs.DataSource = Directory.GetDirectories(textBox_path.Text).ToList();
            foreach (var dir in ETW_Global.excludedDirs) 
            {
                listBox_selectedDirs.Items.Add(dir);
            }
        }
    }
}

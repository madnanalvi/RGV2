using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyFileIOMonitor
{
    public partial class Form_entropy : Form
    {
        public Form_entropy()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                double dbl = CalculateShannonEntropy(FileName, 1000);
                MessageBox.Show(dbl.ToString());
            }
        }
        static double CalculateShannonEntropy(string filePath, int bytesToRead)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            byte[] data = new byte[bytesToRead];

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = fileStream.Read(data, 0, bytesToRead);
                if (bytesRead < bytesToRead)
                {
                    MessageBox.Show("File does not contain enough bytes for calculation.");
                }
            }

            Dictionary<byte, int> frequencyMap = new Dictionary<byte, int>();

            foreach (byte b in data)
            {
                if (frequencyMap.ContainsKey(b))
                {
                    frequencyMap[b]++;
                }
                else
                {
                    frequencyMap[b] = 1;
                }
            }

            int dataSize = data.Length;
            double entropy = 0.0;

            foreach (var entry in frequencyMap)
            {
                double probability = (double)entry.Value / dataSize;
                entropy -= probability * Math.Log(probability, 2);
            }

            return entropy;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string password = "password";
                string inputFileName = openFileDialog1.FileName;
                string outputFilePath = Path.Combine(
            Path.GetDirectoryName(inputFileName),
            Path.GetFileNameWithoutExtension(inputFileName) + "_enc" + Path.GetExtension(inputFileName)
        );
                EncryptFile(inputFileName, outputFilePath, password);
                MessageBox.Show("encryped file saved");
            }
        }
        public static void EncryptFile(string inputFilePath, string outputFilePath, string password)
        {
            using (Aes aes = Aes.Create())
            {
                // Generate a key and IV from the password
                byte[] key, iv;
                GenerateKeyFromPassword(password, aes.KeySize / 8, aes.BlockSize / 8, out key, out iv);

                aes.Key = key;
                aes.IV = iv;

                using (FileStream inputFileStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputFileStream.CopyTo(cryptoStream);
                }
            }
        }

        private static void GenerateKeyFromPassword(string password, int keySize, int blockSize, out byte[] key, out byte[] iv)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SaltIsNotSecret"), 1000))
            {
                key = deriveBytes.GetBytes(keySize);
                iv = deriveBytes.GetBytes(blockSize);
            }
        }
    }
}

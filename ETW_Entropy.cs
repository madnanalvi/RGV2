using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    internal class ETW_Entropy
    {
        public static double CalculateShannonEntropy(string filePath, int bytesToRead)
        {
            if (!File.Exists(filePath))
            {
                return 100;
            }

            byte[] data = new byte[bytesToRead];

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead = fileStream.Read(data, 0, bytesToRead);
                    if (bytesRead < bytesToRead)
                    {
                        bytesToRead = bytesRead;
                    }
                }
            }
            catch (Exception)
            {
                return 99;
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
    }

}

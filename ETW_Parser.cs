using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    internal class ETW_Parser
    {
        ETW_Logger etwlog;
        RichTextBox rtb, rtb2;
        StringBuilder csv;
        public ETW_Parser(RichTextBox _rtb, RichTextBox _rtb2, ETW_Logger _etwlog) 
        {
            rtb = _rtb;
            rtb2 = _rtb2;
            etwlog = _etwlog;
        }

        public void parseAllLogFiles() 
        {
            System.IO.Directory.CreateDirectory(ETW_Global.monitored_Dir+"\\csv");
            ParseIOLog("File I/O Log", ETW_Global.monitored_Dir + "\\FileIOlog.txt", ETW_Global.monitored_Dir + "\\csv\\FileIOlog.csv");
            ParseIOLog("Network Log", ETW_Global.monitored_Dir + "\\Networklog.txt", ETW_Global.monitored_Dir + "\\csv\\Networklog.csv");
            ParseIOLog("Registory Log",ETW_Global.monitored_Dir + "\\Registorylog.txt", ETW_Global.monitored_Dir + "\\csv\\Registorylog.csv");
            ParseIOLog("Thread Log", ETW_Global.monitored_Dir + "\\Threadlog.txt", ETW_Global.monitored_Dir + "\\csv\\Threadlog.csv");
            ParseIOLog("Process Log", ETW_Global.monitored_Dir + "\\Processlog.txt", ETW_Global.monitored_Dir + "\\csv\\Processlog.csv");
        }

        public void ParseIOLog(string logname,string inputFileName, string outputFileName) 
        {
            csv = new StringBuilder();
            if (File.Exists(inputFileName))
            {
                using (StreamReader file = new StreamReader(inputFileName))
                {
                    int counter = 0;
                    string ln;

                    while ((ln = file.ReadLine()) != null)
                    {
                        if (counter == 0) 
                        {
                            string Headers = generateCSVHeaders(ln);
                            csv.Append(Headers);
                            //etwlog.AppendConsole2(rtb, Headers); 
                        }
                        string s = generateCSV(ln);
                        csv.Append(s);
                        //etwlog.AppendConsole2(rtb, s);
                        counter++;
                    }
                    file.Close();
                    etwlog.AppendConsole2(rtb2, logname + " CSV File  having { " + counter + " } Rows created successfully.");
                }
            }
            System.IO.File.WriteAllText(outputFileName, csv.ToString());
        }
        public string generateCSVHeaders(string log)
        {
            StringBuilder sb = new StringBuilder();
            log = log.Replace("\"", "\'");
            log = log.Replace(",", "");            
            string[] str = log.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in str)
            {
                if (word == "<Event") { }
                else if (word.Contains("=")) { sb.Append(word.Split('=')[0] + ","); }
            }
            sb.AppendLine();
            return sb.ToString();
        }
        public string generateCSV(string log) 
        {
            StringBuilder sb = new StringBuilder();
            log = log.Replace("<Event", "");
            log = log.Replace("/>", "");            
            log = log.Replace("\"\"", "0");
            log = log.Replace("\"", "");
            log = log.Replace("=", "= ");
            log = log.Replace(",", "");
            string[] str = log.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in str)
            {
                if (!word.Contains("=")) { sb.Append(word + ","); }
            }
            //sb.Replace(",","",sb.Length-1,1);
            sb.AppendLine();
            return sb.ToString(); 
        } 
    }
}

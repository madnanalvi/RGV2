
using Microsoft.Diagnostics.Tracing.StackSources;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MyFileIOMonitor
{
    public class ETW_Analyze
    {
        ETW_Logger etwlog;
        RichTextBox rtb1, rtb2;
        string csvfile = "";
        string res = "";
        TextBox txt;
        int instanceID = 0;
        public ETW_Analyze(RichTextBox _rtb1, RichTextBox _rtb2, ETW_Logger _etwlog, string _csvfile, TextBox txt, int instanceID)
        {
            etwlog = _etwlog;
            rtb2 = _rtb2;
            rtb1 = _rtb1;
            this.csvfile = _csvfile;
            this.txt = txt;
            this.instanceID = instanceID;
        }
        public void analyzeEvents()
        {
            try
            {
                run_script(ETW_Global.pythonPath, ETW_Global.inference_Script, csvfile, ETW_Global.model, ETW_Global.verbose,ETW_Global.vocab);
            }
            catch (Exception ex) { etwlog.AppendConsole2(rtb2, ex.Message); }
        }
        private void run_script(string python, string inference_Script, string csvfile, string model, string verbose, string vocab)
        {
            string output = "", output1 = "";
            try
            {
                Process psi = new Process();
                string args = inference_Script + " " + verbose +  " " + csvfile + " " + model + " "  + vocab;
                psi.StartInfo = new ProcessStartInfo(python, args) { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = false };
                psi.Start();
                output = psi.StandardOutput.ReadToEnd();
                if (string.IsNullOrEmpty(output))
                {
                    etwlog.AppendConsole2(rtb2, "Alert: Python output is NULL.");
                }
                psi.WaitForExit();
                if (ETW_Global.deleteCSVFiles)
                {
                    File.Delete(csvfile);
                }
            }
            catch (Exception e)
            {
                etwlog.AppendConsole2(rtb2, "\n" + e.Message);
            }            
            try
            {
                output1 = output.Split("||")[1];
            }
            catch (Exception)
            {
                etwlog.AppendConsole2(rtb2, "\n Error parsing output: " + output);
                return;
            }
            if (output1.Trim().Equals("Rows:0"))
            {
                etwlog.AppendConsole2(rtb2, instanceID.ToString()+ ": 0");
                return;
            }
            string maxper = "", pn = "", pid = "", count = "", total = "", s = "", all="" ;
            try
            {
                //||maxper=84.16 pn=wcrt pid=5892 count=324 total=385 nan=0 all=403
                //maxper = (output1.Split(' ')[0]).Split('=')[1].Trim();
                pn = (output1.Split(' ')[1]).Split('=')[1].Trim();
                pid = (output1.Split(' ')[2]).Split('=')[1].Trim();
                count = (output1.Split(' ')[3]).Split('=')[1].Trim();
                total = (output1.Split(' ')[4]).Split('=')[1].Trim();
                all = (output1.Split(' ')[6]).Split('=')[1].Trim();
                s = "BENIGN";                
            }
            catch (Exception) { etwlog.AppendConsole2(rtb2, "\n Parse Error: " + output1); return; }
            //---------------------------------------------------------------------------------------------
            List<string> arr = new List<string>();
            if (ETW_Global.predictedScores.ContainsKey(pid))
            {
                ETW_Global.predictedScores.TryGetValue(pid, out arr);
                arr[1] = (int.Parse(arr[1]) + int.Parse(count)).ToString();
                arr[2] = (int.Parse(arr[2]) + int.Parse(total)).ToString();
                ETW_Global.predictedScores[pid] = arr;
            }
            else 
            {
                arr.Add(pn); arr.Add(count); arr.Add(total);
                ETW_Global.predictedScores.TryAdd(pid, arr); 
            }
            int count1 = int.Parse(arr[1]);
            int total1 = int.Parse(arr[2]);
            decimal t = ((decimal)count1 / total1) * 100;
            maxper = ((float)System.Math.Round(t, 2)).ToString();
            //---------------------------------------------------------------------------------------------
            //Sample size too small
            if (total1<50) { maxper = "0"; }
            //system apps
            //if (pn == "explorer" || pn == "svchost" || pn == "SearchProtocolHost") { maxper = "0"; }
            displayMsg(maxper, arr[0], pid, arr[1], arr[2], all, s);
        }

        private void displayMsg(string maxper, string pn, string pid, string count, string total, string all, string s)
        {
            if (float.Parse(maxper) >= ETW_Global.MALICIOUS_THRESHOLD) { s = "MALICIOUS"; }
            res = instanceID+ ":- Pid: " + pn + "(" + pid + ") is " + s + " having (" + count + "/" + total + ") " + " = " + maxper + "% detection rate.";
            if (float.Parse(maxper) >= ETW_Global.ATTACK_THRESHOLD)
            {
                etwlog.AppendConsole2(rtb2, res, Color.Red);
            }
            else if (float.Parse(maxper) >= ETW_Global.MALICIOUS_THRESHOLD)
            {
                etwlog.AppendConsole2(rtb2, res, Color.YellowGreen);
            }
            else { etwlog.AppendConsole2(rtb2, res, Color.Yellow); }
            try
            {
                if (float.Parse(maxper) >= ETW_Global.ATTACK_THRESHOLD)
                {
                    string msg = " Alert: Ransomware attack Detected.";
                    msg = msg + "\n Malicious Process with Pid: " + pid + " Terminated Successfully.";
                    msg = msg + "\n Backup your critical data and Reinstall your system immediately.";
                    msg = msg + "\n Warning. Do not restart before the backup has been done.";
                    try
                    {
                        Process.GetProcessById(int.Parse(pid)).Kill();
                        MessageBox.Show(msg);
                    }
                    catch (Exception ex) { etwlog.AppendConsole2(rtb2, ex.Message, Color.Yellow); }
                }
            }
            catch (Exception ex)
            { etwlog.AppendConsole2(rtb2, ex.Message, Color.Yellow); }
        }
    }
}

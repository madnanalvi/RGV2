using FastSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyFileIOMonitor.xmltocsv;
using System.Xml.Serialization;
using System.IO;
using System.Xml.XPath;
using System.Data;

namespace MyFileIOMonitor
{
    public static class xmltocsv
    {

        public static void convert(string logname,string inputpath, string outputpath)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(inputpath);
                myDataTable.toCsv(ds.Tables[0], outputpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving "+logname+" , "+ex.Message);
            }
        }

    }
}

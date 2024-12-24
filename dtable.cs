using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    public static class myDataTable
    {
        public static void toCsv(this DataTable dataTable, string filePath)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn column in dataTable.Columns)
            {
                sb.Append(column.ColumnName + ",");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Environment.NewLine);

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append(row[column].ToString() + ",");
                }

                sb.Remove(sb.Length - 1, 1);
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}

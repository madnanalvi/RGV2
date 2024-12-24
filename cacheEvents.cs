using System.Text;

namespace MyFileIOMonitor
{
    public class cacheEvents
    {
        private StringBuilder strBuild;
        private int count = 0;
        public cacheEvents()
        {
            strBuild = new StringBuilder();
        }
        public void AppendLine(String toAppendParam)
        {
            lock (this)
            {
                strBuild.AppendLine(toAppendParam);
                count++;
            }           
        }
        
        public StringBuilder getStringBuilder()
        {
            return strBuild;
        }

        public override string ToString()
        {
            string str = "";
            lock (strBuild)
            {
                str = strBuild.ToString();
            }
            return str;
        }
        public int getLineCount()
        {
            return count;
        }

    }
}

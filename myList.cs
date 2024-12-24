using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFileIOMonitor
{
    public static class myList
    {
        public static string toString<T>(this IList<T> list)
        {
            return string.Join(Environment.NewLine, list);
        }
    }
}

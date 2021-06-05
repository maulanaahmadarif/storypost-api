using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP_API.Models
{
    public class ReturnData
    {
        public bool success { get; set; }
        public string data { get; set; }
        public int rowcount { get; set; }

        public ReturnData(bool _success, string _data, int _rowcount)
        {
            success = _success;
            data = _data;
            rowcount = _rowcount;
        }
    }

    public class ReturnDataCari
    {
        public dynamic[] data { get; set; }
        public dynamic[] children { get; set; }
        public int totalrows { get; set; }
    }


    public class DataNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool expanded { get; set; }
        public bool leaf { get; set; }
        public string nodetype { get; set; }
        public DataNode[] children { get; set; }
    }

    public class DataNodeReturn
    {
        public DataNode[] children { get; set; }
        public int totalrows { get; set; }
    }
}

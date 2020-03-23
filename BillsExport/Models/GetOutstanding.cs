using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class GetOutstanding : IGetOutstanding
    {
        public string uuid { get; set; }
        public string[] status { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}

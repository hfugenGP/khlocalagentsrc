using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IGetOutstanding
    {
        string uuid { get; set; }
        string[] status { get; set; }
        string start { get; set; }
        string end { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IGetPayments
    {
        string uuid { get; set; }
        string start { get; set; }
        string end { get; set; }
    }
}

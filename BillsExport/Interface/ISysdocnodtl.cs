using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface ISysdocnodtl
    {
        int AUTOKEY { get; set; }
        int PARENTKEY { get; set; }
        int NEXTNUMBER { get; set; }
    }
}

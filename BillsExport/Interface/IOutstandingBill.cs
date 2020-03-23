using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IOutstandingBill
    {
        string uuid { get; set; }
        int amount_cents { get; set; }
        string reference_id { get; set; }
        string status { get; set; }
        int outstanding_amount_cents { get; set; }
    }
}

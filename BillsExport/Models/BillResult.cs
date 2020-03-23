using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class BillResult
    {
        public string status { get; set; }
        public List<OutstandingBill> outstandingBills { get; set; }
    }
}

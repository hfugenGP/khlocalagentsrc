using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class OutstandingBill : IOutstandingBill
    {
        public string uuid { get; set; }
        public int amount_cents { get; set; }
        public string reference_id { get; set; }
        public string status { get; set; }
        public int outstanding_amount_cents { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class PaymentResult
    {
        public string status { get; set; }
        public List<AppPayments> result { get; set; }
    }
}

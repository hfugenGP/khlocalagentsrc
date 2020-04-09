using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class AppPayments : IAppPayments
    {
        public string residence { get; set; }

        public string unit_name { get; set; }

        public string code { get; set; }

        public string area { get; set; }

        public string invoice_no { get; set; }

        public string billing_date { get; set; }

        public int amount_cents { get; set; }

        public string description { get; set; }

        public int amount_paid { get; set; }

        public string project_code { get; set; }

        public string payment_date { get; set; } 

        public string uuid { get; set; }
    }
}

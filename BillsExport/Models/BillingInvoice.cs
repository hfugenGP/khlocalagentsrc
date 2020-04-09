using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class BillingInvoice : IInvoice
    {
        public string reference_id { get; set; }
        public string description { get; set; }
        public string project_code { get; set; }
        public string status { get; set; }
        public string due_at { get; set; }
        public string block { get; set; }
        public string currency { get; set; }
        public int amount_cents { get; set; }
        public string billing_date { get; set; }
        public string type { get; set; }
        public int outstanding_amount_cents { get; set; }
        public int reminder_days { get; set; }
        public string floor { get; set; }
        public string unit { get; set; }
        public string residence_uuid { get; set; }
    }
}

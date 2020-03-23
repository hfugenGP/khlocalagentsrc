using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IInvoice
    {
        string reference_id { get; set; }
        string description { get; set; }
        string project_code { get; set; }
        string status { get; set; }
        string due_at { get; set; }
        string block { get; set; }
        string currency { get; set; }
        int amount_cents { get; set; }
        string billing_date { get; set; }
        string type { get; set; }
        int outstanding_amount_cents { get; set; }
        int reminder_days { get; set; }
        string floor { get; set; }
        string unit { get; set; }
        string res_uuid { get; set; }
    }
}

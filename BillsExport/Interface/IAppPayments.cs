using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IAppPayments
    {
        string residence { get; }
        string unit_name { get; }
        string code { get; }
        string area { get; }
        string invoice_no { get; }
        string billing_date { get; }
        int amount_cents { get; }
        string description { get; }
        int amount_paid { get; }
        string project_code { get; }
        string payment_date { get; } 
        string uuid { get;  }
    }
}

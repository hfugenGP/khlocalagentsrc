using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IPayment
    {
        string DOCNO { get; }
        string CODE { get; }
        string DOCDATE { get; }
        string POSTDATE { get; }
        string DESCRIPTION { get; }
        string DOCAMT { get; }
    }
}

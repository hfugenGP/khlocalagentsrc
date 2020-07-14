using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IGlcbdtl
    {
        int DTLKEY { get; set; }
        int DOCKEY { get; set; }
        int SEQ { get; set; }
        string AGENT { get; set; }
        string PROJECT { get; set; }
        string CODE { get; set; }
        string DESCRIPTION { get; set; }
        string GST_DOCDATE { get; set; }
        string GST_DOCNO { get; set; }
        string COMPANYNAME { get; set; }
        string REGISTERNO { get; set; }
        string GSTNO { get; set; }
        string PERMITNO { get; set; }
        string COUNTRY { get; set; }
        string TAX { get; set; }
        string TARIFF { get; set; }
        string TAXRATE { get; set; }
        float TAXAMT { get; set; }
        float LOCALTAXAMT { get; set; }
        int TAXINCLUSIVE { get; set; }
        float AMOUNT { get; set; }
        float LOCALAMOUNT { get; set; }
        string CURRENCYCODE { get; set; }
        float CURRENCYRATE { get; set; }
        float CURRENCYAMOUNT { get; set; }
    }
}

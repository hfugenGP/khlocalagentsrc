using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IGlcb
    {
        int DOCKEY { get; set; }
        string DOCNO { get; set; }
        string DOCTYPE { get; set; }
        string DOCDATE { get; set; }
        string POSTDATE { get; set; }
        string TAXDATE { get; set; }
        string DESCRIPTION { get; set; }
        string DESCRIPTION2 { get; set; }
        string PAYMENTMETHOD { get; set; }
        string AREA { get; set; }
        string AGENT { get; set; }
        string PROJECT { get; set; }
        string JOURNAL { get; set; }
        string CHEQUENUMBER { get; set; }
        string CURRENCYCODE { get; set; }
        float CURRENCYRATE { get; set; }
        float BANKCHARGE { get; set; }
        float DOCAMT { get; set; }
        float LOCALDOCAMT { get; set; }
        string FROMDOCTYPE { get; set; }
        string BOUNCEDDATE { get; set; }
        int GLTRANSID { get; set; } 
        string CANCELLED { get; set; }
        int DEPOSITKEY { get; set; }
        int UPDATECOUNT { get; set; }
        int PRINTCOUNT { get; set; }
        string ATTACHMENTS { get; set; }
        string NOTE { get; set; }
    }
}

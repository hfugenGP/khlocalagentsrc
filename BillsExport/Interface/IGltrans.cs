using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IGltrans
    {
        int DOCKEY { get; set; }
        int GLTRANSID { get; set; }
        string CODE { get; set; }
        string DOCDATE { get; set; }
        string POSTDATE { get; set; }
        string TAXDATE { get; set; }
        string AREA { get; set; }
        string AGENT { get; set; }
        string PROJECT { get; set; }
        string TAX { get; set; }
        string JOURNAL { get; set; }
        string CURRENCYCODE { get; set; }
        float CURRENCYRATE { get; set; }
        string DESCRIPTION { get; set; }
        string DESCRIPTION2 { get; set; }
        float DR { get; set; }
        float CR { get; set; }
        float LOCALDR { get; set; }
        float LOCALCR { get; set; }
        string REF1 { get; set; }
        string REF2 { get; set; }
        string FROMDOCTYPE { get; set; }
        int FROMKEY { get; set; }
        string TABLETYPE { get; set; }
        string RECONDATE { get; set; }
        string CANCELLED { get; set; }
        int AUTOPOST { get; set; }
        string NONCE { get; set; }
        string DIGEST { get; set; }
    }
}

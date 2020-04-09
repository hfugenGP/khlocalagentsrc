using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Interface
{
    interface IApknockoff
    {
        int DOCKEY { get; set; }
        string FROMDOCTYPE { get; set; }
        int FROMDOCKEY { get; set; } 
        string TODOCTYPE { get; set; } 
        int TODOCKEY { get; set; } 
        float KOAMT { get; set; } 
        float ACTUALLOCALKOAMT { get; set; } 
        float LOCALKOAMT { get; set; } 
        string KOTAXDATE { get; set; } 
        float GAINLOSS { get; set; } 
        string GAINLOSSPOSTDATE { get; set; }
    }
}

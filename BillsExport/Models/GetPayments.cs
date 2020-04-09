using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class GetPayments : IGetPayments
    {
        public string uuid { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}

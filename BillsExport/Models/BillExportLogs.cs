using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class BillExportLogs
    {
        public string residence_uuid
        {
            get;
            set;
        }

        public DateTime created_at
        {
            get;
            set;
        }
    }
}

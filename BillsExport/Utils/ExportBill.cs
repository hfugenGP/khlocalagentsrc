using BillsExport.Models;
using BillsExport.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class ExportBill
    {
        public static void dump(List<BillingInvoice> invoices, int index)
        {
            KipleHomeService kipleApi = new KipleHomeService();
            try
            {
                int batchSize = 10;
                List<BillingInvoice> currentBatch = Excerpt.extract(invoices, index, index + batchSize);

                if (currentBatch.Count() != 0)
                {
                    kipleApi.exportBill(currentBatch);
                    ExportBill.dump(invoices, index + batchSize);
                }
                else
                {
                    Console.WriteLine("Invoice export completed.\n");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("==========================================\n");
                Console.WriteLine("From Export:\n");
                Console.WriteLine(ex.Message);
                Console.WriteLine("==========================================\n");
            }
        }
    }
}

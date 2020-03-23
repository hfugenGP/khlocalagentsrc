using BillsExport.Models;
using BillsExport.Utils;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport
{
    public class BillsMain
    {
        public void buildExport()
        {
            Fbexplore fbexplore = new Fbexplore(ConfigurationManager.AppSettings["estream"]);
            if (!String.IsNullOrEmpty(fbexplore.LatestPath))
            {
                Console.WriteLine($"Attempting to connect to latest DB : {fbexplore.LatestPath}\n");

                try
                {
                    Console.WriteLine($"Checking invoices for the past {ConfigurationManager.AppSettings["datespan"]}\n");

                    Ariv ariv = new Ariv(fbexplore.LatestPath);
                    FbDataReader invoices = ariv.queryInvoice(); 

                    if (invoices.HasRows)
                    {
                        PosthocBill posthocBill = new PosthocBill(ref invoices);

                        if (posthocBill.InvoiceLst.Count() != 0)
                        {
                            Console.WriteLine($"Record count: {posthocBill.InvoiceLst.Count()}\n");
                            ExportBill.dump(posthocBill.InvoiceLst, 0);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No invoice(s) found for the past {ConfigurationManager.AppSettings["datespan"]}\n");
                    }

                    // Check for payments. 
                    Console.WriteLine("Checking for pending payments...\n");
                    AssertBills assertBills = new AssertBills();
                    if (assertBills.Bills.Count != 0)
                    {
                        Console.WriteLine($"Outstanding Bills: {assertBills.Bills.Count}");
                        UpdateBills updateBills = new UpdateBills(assertBills.Bills, fbexplore.LatestPath);
                        updateBills.exportPayment();
                    }
                    else
                    {
                        Console.WriteLine("No outstanding bills.");
                    }

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

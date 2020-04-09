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
                    // ===================================
                    //  Export new invoices to kiple home
                    // ===================================

                    Console.WriteLine($"Checking invoices for the past {ConfigurationManager.AppSettings["datespan"]} days.\n");

                    Ariv ariv = new Ariv(fbexplore.LatestPath);
                    FbDataReader invoices = ariv.queryInvoice();

                    if (invoices.HasRows)
                    {
                        PosthocBill posthocBill = new PosthocBill(ref invoices);

                        if (posthocBill.InvoiceLst.Count() != 0)
                        {
                            Console.WriteLine($"Invoice count: {posthocBill.InvoiceLst.Count()}\n");
                            ExportBill.dump(posthocBill.InvoiceLst, 0);
                        }
                        else
                        {
                            Console.WriteLine($"No invoice(s) found for the past {ConfigurationManager.AppSettings["datespan"]} days.\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No invoice(s) found for the past {ConfigurationManager.AppSettings["datespan"]} days.\n");
                    } 

                    // ===================================================================================
                    //  Check kiple home for pending invoices. Export payments back to kiple home if any.
                    // ===================================================================================

                    Console.WriteLine("Checking for pending payments: Connecting to Kiple Home ...\n");
                    AssertBills assertBills = new AssertBills();
                    if (!(assertBills.Bills is null) && assertBills.Bills.Count != 0)
                    {
                        Console.WriteLine($"Outstanding Bills from KipleHome : {assertBills.Bills.Count} \n");
                        UpdateBills updateBills = new UpdateBills(assertBills.Bills, fbexplore.LatestPath);
                        updateBills.exportPayment();
                    }
                    else
                    {
                        Console.WriteLine("No outstanding bills. \n");
                    }

                    // =====================================================
                    //  Check kiple home for payments made through KipleBiz
                    // ===================================================== 

                    Console.WriteLine("Checking for payments made via KipleHome App ... \n");
                    AdyenPayment adyenPayment = new AdyenPayment();
                    if (!(adyenPayment.Payments is null) && adyenPayment.Payments.Count != 0)
                    {
                        Console.WriteLine($"Inserting {adyenPayment.Payments.Count} payment(s) to local database ... \n");
                        MakePayment makePayment = new MakePayment(adyenPayment.Payments, fbexplore.LatestPath);
                        makePayment.insertPayments();
                    }
                    else 
                    {
                        Console.WriteLine("No payments made through KipleHome App. \n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====================================================================================\n");
                    Console.WriteLine("From Main:\n");                     
                    Console.WriteLine(ex);
                    Console.WriteLine("====================================================================================\n");
                }
            }
        }
    }
}

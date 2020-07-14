using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchWork;
using System.Configuration;
using BillsExport;
using System.Diagnostics;

namespace khLocalAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("kipleHome Local Agent\n");

            Console.WriteLine("Checking for updates...\n");

            // Check for updates
            // Build build = new Build();
            // bool uptodate = build.pullRequest(ConfigurationManager.AppSettings["target"]);
            bool uptodate = true;

            if (uptodate)
            {
                Console.WriteLine("App is up to date...\n");

                KillApp();

                BillsMain billsMain = new BillsMain();
                billsMain.buildExport();
            } 
            else
            {
               Console.WriteLine("Close this application then run the PatchTool.\n");
            }

            // Console.ReadKey();
        }

        /**
         * Make sure no other SQLAcc and/or Flame Robbin are running
         * 
         * Reference: AccountingLink sample code from SQL Accounting. 
         * 
         * Added Flame Robbin as it is being used for testing.
         */
        static void KillApp()
        {
            string[] appConflicts = {"flamerobin", "SQLACC", "SQLacc"};

            Console.WriteLine("Closing process(es) that might conflict with local agent \n");
            try
            {
                foreach (Process prc in Process.GetProcesses())
                {
                    // Check if process is in list of Applications in conflict with local agent.
                    if (Array.IndexOf(appConflicts, prc.ProcessName) >= 0) 
                    {
                        Console.WriteLine($"Closing process: {prc.ProcessName} -> {prc.MainWindowTitle}\n"); 
                        prc.Kill();
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}

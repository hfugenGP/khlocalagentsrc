using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatchWork;
using System.Configuration;
using BillsExport;

namespace khLocalAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("kipleHome Local Agent\n");

            Console.WriteLine("Checking for updates...\n");
            // Check for updates
            Build build = new Build();
            bool uptodate = build.pullRequest(ConfigurationManager.AppSettings["target"]);

            if (uptodate)
            {
                Console.WriteLine("App is up to date...\n");

                BillsMain billsMain = new BillsMain();
                billsMain.buildExport();
            }
            
            Console.ReadKey();
        }
    }
}

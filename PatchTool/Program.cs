using PatchWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace PatchTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = ConfigurationManager.AppSettings["patchdir"];
            string repo = ConfigurationManager.AppSettings["repo"];
            Build build = new Build();

            Console.WriteLine($"Checking if {target} exists ... \n");

            if (Directory.Exists(target))
            {
                Console.WriteLine("Fetching...");
                
                build.fetchRequest(ConfigurationManager.AppSettings["patchdir"]);
                bool uptodate = build.pullRequest(ConfigurationManager.AppSettings["patchdir"]); 

                if (uptodate)
                {
                    Console.WriteLine("Patch successful, you may close this window.");
                }
            } 
            else
            {
                // Create target directory and clone 
                DirectoryInfo directoryInfo = Directory.CreateDirectory(target);
                if (directoryInfo.Exists)
                {
                    // Clone 
                    Console.WriteLine($"{target} successfully created. \n");
                    Console.WriteLine($"Cloning local agent to {target}. \n");
                    string path = build.cloneRequest(repo, target); 
                    if (path != "")
                    {
                        Console.WriteLine("Clone successful.\n");
                    }
                }
            }
            
            Console.ReadKey();
        }
    }
}

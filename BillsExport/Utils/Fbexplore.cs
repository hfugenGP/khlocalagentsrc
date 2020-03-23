using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class Fbexplore
    {
        private string _dbpath;

        public string LatestPath
        {
            get;
            private set;
        }

        public Fbexplore(string dbpath)
        {
            _dbpath = dbpath;
            _setDbpath();
        }

        private void _setDbpath()
        {
            if (!String.IsNullOrEmpty(_dbpath) && Directory.Exists(_dbpath))
            {

                string[] files = Directory.GetFiles(_dbpath);
                if (files.Length != 0)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_dbpath);
                    FileInfo fileInfo = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
                    string latestPath = fileInfo.ToString();
                    if (!String.IsNullOrEmpty(latestPath))
                    {
                        LatestPath = $"{Directory.GetCurrentDirectory()}\\{latestPath}";
                        System.IO.File.Copy($"{_dbpath}\\{latestPath}", LatestPath, true);
                    }
                }                
            }
        }
    }
}

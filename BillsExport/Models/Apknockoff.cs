using BillsExport.Dbconn;
using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class Apknockoff : Fbdbconn, IApknockoff
    {
        private string _table;

        public Apknockoff(string dbpath) : base(dbpath)
        {
            _table = "AR_KNOCKOFF";
        }

        public int DOCKEY { get; set; }
        public string FROMDOCTYPE { get; set; }
        public int FROMDOCKEY { get; set; }
        public string TODOCTYPE { get; set; }
        public int TODOCKEY { get; set; }
        public float KOAMT { get; set; }
        public float ACTUALLOCALKOAMT { get; set; }
        public float LOCALKOAMT { get; set; }
        public string KOTAXDATE { get; set; }
        public float GAINLOSS { get; set; }
        public string GAINLOSSPOSTDATE { get; set; }

        public void Add()
        {
            try
            {
                string columns = "";
                string values = "";
                foreach (PropertyInfo prop in typeof(Apknockoff).GetProperties())
                {
                    if (prop.GetValue(this) != null)
                    {
                        columns += prop.Name + ",";
                        values += prop.GetValue(this) + ",";
                    }
                }
                if (columns != "" && values != "")
                {
                    insert(_table, columns.Substring(0, columns.Length - 1), values.Substring(0, values.Length - 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }

        public int nextDockey()
        {
            return NextValueFor("AR_KNOCKOFF");
        }
    }
}

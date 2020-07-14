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
    class Glcbdtl : Fbdbconn, IGlcbdtl
    {
        private string _table;

        public Glcbdtl(string dbpath) : base(dbpath)
        {
            _table = "GL_CBDTL";
        }

        public int DTLKEY { get; set; }
        public int DOCKEY { get; set; }
        public int SEQ { get; set; }
        public string AGENT { get; set; }
        public string PROJECT { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string GST_DOCDATE { get; set; }
        public string GST_DOCNO { get; set; }
        public string COMPANYNAME { get; set; }
        public string REGISTERNO { get; set; }
        public string GSTNO { get; set; }
        public string PERMITNO { get; set; }
        public string COUNTRY { get; set; }
        public string TAX { get; set; }
        public string TARIFF { get; set; }
        public string TAXRATE { get; set; }
        public float TAXAMT { get; set; }
        public float LOCALTAXAMT { get; set; }
        public int TAXINCLUSIVE { get; set; }
        public float AMOUNT { get; set; }
        public float LOCALAMOUNT { get; set; }
        public string CURRENCYCODE { get; set; }
        public float CURRENCYRATE { get; set; }
        public float CURRENCYAMOUNT { get; set; }

        public void Add()
        {
            try
            {
                string columns = "";
                string values = "";
                foreach (PropertyInfo prop in typeof(Glcbdtl).GetProperties())
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
            }
        }

        public int nextDTLKEY()
        {
            return nextDTLKEY(_table);
        }
    }
}

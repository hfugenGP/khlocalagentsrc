using BillsExport.Dbconn;
using BillsExport.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace BillsExport.Models
{
    class Glcb : Fbdbconn, IGlcb
    {
        private string _table;

        public Glcb(string dbpath) : base(dbpath)
        {
            _table = "GL_CB";
        }

        public int DOCKEY { get; set; }
        public string DOCNO { get; set; }
        public string DOCTYPE { get; set; }
        public string DOCDATE { get; set; }
        public string POSTDATE { get; set; }
        public string TAXDATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESCRIPTION2 { get; set; }
        public string PAYMENTMETHOD { get; set; }
        public string AREA { get; set; }
        public string AGENT { get; set; }
        public string PROJECT { get; set; }
        public string JOURNAL { get; set; }
        public string CHEQUENUMBER { get; set; }
        public string CURRENCYCODE { get; set; }
        public float CURRENCYRATE { get; set; }
        public float BANKCHARGE { get; set; }
        public float DOCAMT { get; set; }
        public float LOCALDOCAMT { get; set; }
        public string FROMDOCTYPE { get; set; }
        public string BOUNCEDDATE { get; set; }
        public int GLTRANSID { get; set; }
        public string CANCELLED { get; set; }
        public int DEPOSITKEY { get; set; }
        public int UPDATECOUNT { get; set; }
        public int PRINTCOUNT { get; set; }
        public string ATTACHMENTS { get; set; }
        public string NOTE { get; set; }

        public void Add()
        {
            try
            {
                string columns = "";
                string values = "";
                foreach (PropertyInfo prop in typeof(Glcb).GetProperties())
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

        public int nextDockey()
        {
            return NextValueFor("GL_CB");
        }

        public int nextTransid()
        {
            return NextValueFor("GL_CB_ID");
        }

        public int nextFromkey()
        {
            return NextFromKey(_table);
        }
    }
}

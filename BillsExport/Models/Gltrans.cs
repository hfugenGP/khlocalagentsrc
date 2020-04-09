using BillsExport.Dbconn;
using BillsExport.Interface;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class Gltrans  : Fbdbconn, IGltrans
    {
        private string _table;

        public Gltrans(string dbpath) : base(dbpath)
        {
            _table = "GL_TRANS";
        }

        public int DOCKEY { get; set; }
        public int GLTRANSID { get; set; }
        public string CODE { get; set; }
        public string DOCDATE { get; set; }
        public string POSTDATE { get; set; }
        public string TAXDATE { get; set; }
        public string AREA { get; set; }
        public string AGENT { get; set; }
        public string PROJECT { get; set; }
        public string TAX { get; set; }
        public string JOURNAL { get; set; }
        public string CURRENCYCODE { get; set; }
        public float CURRENCYRATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESCRIPTION2 { get; set; }
        public float DR { get; set; }
        public float CR { get; set; }
        public float LOCALDR { get; set; }
        public float LOCALCR { get; set; }
        public string REF1 { get; set; }
        public string REF2 { get; set; }
        public string FROMDOCTYPE { get; set; }
        public int FROMKEY { get; set; }
        public string TABLETYPE { get; set; }
        public string RECONDATE { get; set; }
        public string CANCELLED { get; set; }
        public int AUTOPOST { get; set; }
        public string NONCE { get; set; }
        public string DIGEST { get; set; }

        public void Add()
        {
            try
            {
                string columns = "";
                string values = "";
                foreach (PropertyInfo prop in typeof(Gltrans).GetProperties())
                {
                    if (prop.GetValue(this) != null)
                    {
                        columns += prop.Name + ",";
                        values += prop.GetValue(this) + ",";
                    }
                }
                if (columns != "" && values != "")
                {
                    insert(_table, columns.Substring(0, columns.Length -1), values.Substring(0, values.Length - 1));
                }
                           }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int nextDockey()
        {
            return NextValueFor("GL_TRANS");
        }

        public int nextTransid()
        {
            return NextValueFor("GL_TRANS_ID");
        }

        public int nextFromkey()
        {
            return NextFromKey(_table);
        }
    }
}

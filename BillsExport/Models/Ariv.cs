using BillsExport.Dbconn;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class Ariv : Fbdbconn
    {
        private DateTime _endDate;
        private DateTime _startDate;
        private string _table;

        public Ariv(string dbpath) : base(dbpath)
        {

            _endDate = DateTime.Now.AddDays(int.Parse(ConfigurationManager.AppSettings["datespan"]));
            // _endDate = DateTime.Now;
            // _startDate = _endDate.AddDays(int.Parse($"-{ConfigurationManager.AppSettings["datespan"]}"));
            _startDate = DateTime.Now.AddDays(int.Parse($"-{ConfigurationManager.AppSettings["datespan"]}"));
            _table = "AR_IV";
        }

        public int DOCKEY
        {
            get;
            set;
        }

        public string DOCNO
        {
            get;
            set;
        }

        public float DOCAMT
        {
            get;
            set;
        }

        public float PAYMENTAMT
        {
            get;
            set;
        }

        public FbDataReader queryInvoice()
        {
            string filter = $"POSTDATE BETWEEN '{_startDate.ToString("dd.MM.yyyy")}' AND '{_endDate.ToString("dd.MM.yyyy")}'"; 
            return this.select(_table, filter);
        }

        public void checkPayment(string invoiceno)
        {
            DOCKEY = 0;
            DOCNO = "";
            DOCAMT = 0;
            PAYMENTAMT = 0;
            string filter = $"DOCNO = '{invoiceno}'";
            FbDataReader dataReader = this.select(_table, filter);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    try
                    {
                        DOCKEY = (values[0] != null ? int.Parse(values[0].ToString()) : 0);
                        DOCNO = (values[1] != null ? values[1].ToString() : "");
                        DOCAMT = (values[16] != null ? float.Parse(values[16].ToString()) : 0);
                        PAYMENTAMT = (values[18] != null ? float.Parse(values[18].ToString()) : 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public void Update(string invoiceno)
        {
            try
            {
                string values = "";
                foreach (PropertyInfo prop in typeof(Ariv).GetProperties())
                {
                    if (prop.GetValue(this) != null)
                    {
                        if (prop.PropertyType.ToString() == "System.String" )
                        {
                            values += prop.Name + " = '" + prop.GetValue(this) + "',";
                        }
                        else
                        {
                            values += prop.Name + " = " + prop.GetValue(this) + ",";
                        }
                    }
                }
                if (!String.IsNullOrEmpty(values))
                {
                    update(_table, values.Substring(0, values.Length - 1), $"DOCNO = '{invoiceno}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

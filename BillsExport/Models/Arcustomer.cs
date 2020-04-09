using BillsExport.Dbconn;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Models
{
    class Arcustomer : Fbdbconn
    {
        private string _table;

        public Arcustomer(string dbpath) : base(dbpath)
        {
            _table = "AR_CUSTOMER";
        }

        public string CODE
        {
            get;
            private set;
        }

        public string COMPANYNAME
        {
            get;
            private set;
        }

        public float OUTSTANDING
        {
            get;
            set;
        }

        public void findBycode(string code)
        {
            string filter = $"CODE = '{code}'";
            FbDataReader dataReader = this.select(_table, filter);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    try
                    {
                        CODE = (values[0] != null ? values[0].ToString() : "");
                        COMPANYNAME = (values[2] != null ? values[2].ToString() : "");
                        OUTSTANDING = (values[13] != null ? float.Parse(values[13].ToString()) : 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public void Update(string code)
        {
            try
            {
                string values = "";
                foreach (PropertyInfo prop in typeof(Arcustomer).GetProperties())
                {
                    if (prop.GetValue(this) != null)
                    {
                        if (prop.PropertyType.ToString() == "System.String")
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
                    update(_table, values.Substring(0, values.Length - 1), $"CODE = '{code}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

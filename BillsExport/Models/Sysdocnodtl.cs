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
    class Sysdocnodtl : Fbdbconn, ISysdocnodtl
    {
        private string _table;

        public Sysdocnodtl(string dbpath) : base(dbpath)
        {
            _table = "SY_DOCNO_DTL";
        }

        public int AUTOKEY { get; set; }
        public int PARENTKEY { get; set; } 
        public int NEXTNUMBER { get; set; } 

        public void nxtOffclrcpt()
        {
            AUTOKEY = 0;
            PARENTKEY = 0;
            NEXTNUMBER = 0;
            string filter = $"PARENTKEY = 1";
            FbDataReader dataReader = this.select(_table, filter);
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    object[] values = new object[dataReader.FieldCount];
                    dataReader.GetValues(values);
                    try
                    {
                        AUTOKEY = (values[0] != null ? int.Parse(values[0].ToString()) : 0);
                        PARENTKEY = (values[1] != null ? int.Parse(values[1].ToString()) : 0);
                        NEXTNUMBER = (values[4] != null ? int.Parse(values[4].ToString()) : 0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        public void Update(int autokey)
        {
            try
            {
                string values = "";
                foreach (PropertyInfo prop in typeof(Sysdocnodtl).GetProperties())
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
                    update(_table, values.Substring(0, values.Length - 1), $"AUTOKEY = {autokey}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}

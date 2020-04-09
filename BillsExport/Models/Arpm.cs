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
    class Arpm : Fbdbconn, IPayment
    {
        private string _table;

        public Arpm(string dbpath) : base(dbpath)
        {
            _table = "AR_PM";
        }

        public int DOCKEY
        {
            get;
            set;
        }

        public string DOCNO { 
            get; 
            set; 
        }

        public string CODE { 
            get; 
            set; 
        }

        public string DOCDATE { 
            get; 
            set; 
        }
        
        public string POSTDATE { 
            get; 
            set; 
        }

        public string TAXDATE
        {
            get;
            set;
        }

        public string DESCRIPTION { 
            get; 
            set; 
        }

        public string AREA
        {
            get;
            set;
        }

        public string PAYMENTMETHOD
        {
            get;
            set;
        }

        public string JOURNAL
        {
            get;
            set;
        }

        public float CURRENCYRATE
        {
            get;
            set;
        }

        public float BANKCHARGE
        {
            get;
            set;
        }

        public float DOCAMT 
        { 
            get; 
            set; 
        }

        public float LOCALDOCAMT
        {
            get;
            set;
        }

        public float UNAPPLIEDAMT
        {
            get;
            set;
        }

        public int GLTRANSID
        {
            get;
            set;
        }

        public string CANCELLED
        {
            get;
            set;
        }

        public int NONREFUNDABLE
        {
            get;
            set;
        }

        public string CHEQUENUMBER
        {
            get;
            set;
        }

        /*public string UDF_POSTDN
        {
            get;
            set;
        }*/

        /**
         * Check latest payment
         * 
         */
        public void payment(string docno)
        {
            string condition = $"DOCNO = '{docno}' ORDER BY DOCKEY DESC ROWS 1 TO 1";
            FbDataReader fbDataReader = this.select(_table, condition);
            _setPaymentInfo(fbDataReader);
        } 

        private void _setPaymentInfo(FbDataReader fbDataReader)
        {
            if (fbDataReader.HasRows)
            {
                while(fbDataReader.Read())
                {
                    var values = new object[fbDataReader.FieldCount];
                    fbDataReader.GetValues(values);
                    DOCNO = values[0].ToString();
                    CODE = values[1].ToString();
                    DOCDATE = values[2].ToString();
                    POSTDATE = values[3].ToString();
                    DESCRIPTION = values[4].ToString();
                    DOCAMT = float.Parse(values[5].ToString());
                }
            }
        }

        public void Add()
        {
            try
            {
                string columns = "";
                string values = "";
                foreach (PropertyInfo prop in typeof(Arpm).GetProperties())
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
            return NextValueFor(_table);
        }


        /*===============================================================
         * 
         *      DEPRECATED
         * 
         *===============================================================/

        /**
         * This method is deprecated please use the OR generator in the 
         * Sysdocnodtl class
         */
        public string nextOR()
        {
            string offRec = "OR17-";
            string temp = "";
            try
            {
                string query = "SELECT FIRST 1 DOCNO FROM AR_PM WHERE DOCNO LIKE 'OR%' ORDER BY DOCKEY DESC; ";
                FbDataReader dataReader = rawQuery(query);
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        object[] values = new object[dataReader.FieldCount];
                        dataReader.GetValues(values);
                        try
                        {
                            temp = _incrementRcpt(values[0].ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return offRec + temp;
        }

        /*===============================================================
         * 
         *      DEPRECATED
         * 
         *===============================================================/

        /**
         * This method is deprecated please use the OR generator in the 
         * Sysdocnodtl class
         */
        private string _incrementRcpt(string reciept)
        {
            string inc = "";
            char pad = '0';
            if (!String.IsNullOrEmpty(reciept))
            {
                string[] aRcpt = reciept.Split('-');
                if (aRcpt.Length != 0)
                {
                    inc = (int.Parse(aRcpt[1].ToString()) + 1).ToString();
                }
            }
            return inc.PadLeft(5, pad);
        }
    }
}

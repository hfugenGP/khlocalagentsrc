using BillsExport.Dbconn;
using BillsExport.Interface;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string DOCNO { 
            get; 
            private set; 
        }

        public string CODE { 
            get; 
            private set; 
        }

        public string DOCDATE { 
            get; 
            private set; 
        }

        public string POSTDATE { 
            get; 
            private set; 
        }

        public string DESCRIPTION { 
            get; 
            private set; 
        }

        public string DOCAMT { 
            get; 
            private set; 
        }

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
                    DOCAMT = values[5].ToString();
                }
            }
        }
    }
}

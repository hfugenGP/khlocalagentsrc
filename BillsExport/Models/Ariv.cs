using BillsExport.Dbconn;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            _endDate = DateTime.Now;
            _startDate = _endDate.AddDays(int.Parse($"-{ConfigurationManager.AppSettings["datespan"]}"));
            _table = "AR_IV";
        } 

        public FbDataReader queryInvoice()
        {
            string filter = $"POSTDATE BETWEEN '{_startDate.ToString("dd.MM.yyyy")}' AND '{_endDate.ToString("dd.MM.yyyy")}'";
            return this.select(_table, filter);
        }
    }
}

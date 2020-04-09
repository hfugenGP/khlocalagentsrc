using BillsExport.Models;
using BillsExport.Service;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace BillsExport.Utils
{
    class AssertBills
    {
        private string _uuid;
        private string[] _status;
        private DateTime _start;
        private DateTime _end;

        public AssertBills()
        {
            _uuid = ConfigurationManager.AppSettings["uuid"];
            _status = new string[] { "PENDING", "OVERDUE" };
            _end = DateTime.Now;
            _start = _end.AddDays(int.Parse("-" + ConfigurationManager.AppSettings["datespan"]));
            _checkBills();
        }

        private void _checkBills()
        {
            GetOutstanding getOutstanding = new GetOutstanding();
            getOutstanding.uuid = _uuid;
            getOutstanding.status = _status;
            getOutstanding.start = _start.ToString("yyyy-MM-dd");
            getOutstanding.end = _end.ToString("yyyy-MM-dd");
            KipleHomeService kipleHomeService = new KipleHomeService();
            IRestResponse restResponse = kipleHomeService.inquirePending(getOutstanding);
            _constructList(restResponse);
        }

        public List<OutstandingBill> Bills
        {
            get;
            private set;
        }

        private void _constructList(IRestResponse restResponse)
        {
            if (restResponse.StatusCode == HttpStatusCode.OK && restResponse.Content.Length != 0)
            {
                BillResult billResult = JsonConvert.DeserializeObject<BillResult>(restResponse.Content); 
                Bills = billResult.result;
            }
        }
    }
}

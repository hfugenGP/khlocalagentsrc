using BillsExport.Models;
using BillsExport.Service;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class AdyenPayment
    {
        private string _uuid;
        private DateTime _now;
        private DateTime _start;
        private DateTime _end;

        public AdyenPayment()
        {
            _uuid = ConfigurationManager.AppSettings["uuid"]; 
            _now = DateTime.Now;

            // Retrieve payments using datespan 
            _end = DateTime.Now;
            _start = _end.AddDays(int.Parse($"-{ConfigurationManager.AppSettings["datespan"]}"));

            _checkPayments();
        } 

        public List<AppPayments> Payments
        {
            get;
            private set;
        }

        private void _checkPayments()
        {
            GetPayments getPayments = new GetPayments();
            getPayments.uuid = _uuid;
            getPayments.start = _start.ToString("yyyy-MM-dd");
            getPayments.end = _end.ToString("yyyy-MM-dd");

            Console.WriteLine($"Checking pamyents made between {getPayments.start} and {getPayments.end} \n");

            KipleHomeService kipleHomeService = new KipleHomeService();
            IRestResponse restResponse = kipleHomeService.checkPayments(getPayments);
            _constructList(restResponse);
        }

        private void _constructList(IRestResponse restResponse)
        {
            if (restResponse.StatusCode == HttpStatusCode.OK && restResponse.Content.Length != 0)
            {
                PaymentResult paymentResult = JsonConvert.DeserializeObject<PaymentResult>(restResponse.Content);
                Payments = paymentResult.result;
            }
        }
    }
}

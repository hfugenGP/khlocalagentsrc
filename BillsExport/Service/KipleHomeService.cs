using BillsExport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Reflection;

namespace BillsExport.Service
{
    class KipleHomeService
    {
        private static string _url;
        private static string _payments;
        private static string _receipts;
        private string _apiKey;

        public KipleHomeService()
        {
            _url = ConfigurationManager.AppSettings["api"];
            _payments = ConfigurationManager.AppSettings["payments"];
            _receipts = ConfigurationManager.AppSettings["receipts"];
            _apiKey = "5Syw49JVgmCDrGv5QBDbxtDvpTR2XxkF36Vr4EMVkvDVecJX";
        }

        public void exportBill(IList<BillingInvoice> invoices)
        {
            if (invoices != null && invoices.Count != 0)
            {
                var body = new
                {
                    resource = invoices
                };
                RestClient restClient = new RestClient(_url);
                RestRequest restRequest = new RestRequest("", Method.POST);
                restRequest.AddHeader("X-Application-Key", _apiKey);
                restRequest.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                if (restResponse.StatusCode != 0)
                {
                    Console.WriteLine($"{restResponse.Content}\n");
                }
                else
                {
                    Console.WriteLine($"Failed to export billing: {restResponse.ErrorMessage}\n");
                }
            }
        }

        public IRestResponse inquirePending(GetOutstanding body)
        {
            RestClient restClient = new RestClient(_url);
            RestRequest restRequest = new RestRequest("", Method.GET);
            restRequest.AddHeader("X-Application-Key", _apiKey);
            foreach (PropertyInfo prop in body.GetType().GetProperties())
            {
                string param = "";
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(string[]))
                {
                    string[] status = (string[])prop.GetValue(body, null);
                    for (int i = 0; i < status.Length; i++)
                    {
                        param += status[i] + ",";
                    }

                    // Remove trailing comma
                    param = param.Substring(0, param.Length - 1);
                }
                else
                {
                    param = prop.GetValue(body, null).ToString();
                }

                restRequest.AddParameter(prop.Name, param, ParameterType.QueryString);
            }
            IRestResponse restResponse = restClient.Get(restRequest);
            return restResponse;
        }

        public IRestResponse checkPayments(GetPayments body)
        {
            RestClient restClient = new RestClient(_payments);
            RestRequest restRequest = new RestRequest("", Method.GET);
            restRequest.AddHeader("X-Application-Key", _apiKey);
            foreach (PropertyInfo prop in body.GetType().GetProperties())
            {
                string param = "";
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(string[]))
                {
                    string[] status = (string[])prop.GetValue(body, null);
                    for (int i = 0; i < status.Length; i++)
                    {
                        param += status[i] + ",";
                    }

                    // Remove trailing comma
                    param = param.Substring(0, param.Length - 1);
                }
                else
                {
                    param = prop.GetValue(body, null).ToString();
                }

                restRequest.AddParameter(prop.Name, param, ParameterType.QueryString);
            }
            IRestResponse restResponse = restClient.Get(restRequest);
            return restResponse;
        }

        public void exportPayments(IList<OutstandingBill> payments)
        {
            if (payments != null && payments.Count != 0)
            {
                var body = new
                {
                    resource = payments
                };
                RestClient restClient = new RestClient(_url);
                RestRequest restRequest = new RestRequest("", Method.PATCH);
                restRequest.AddHeader("X-Application-Key", _apiKey);
                restRequest.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                Console.WriteLine($"{restResponse.Content}\n");
            }
        }

        public void exportReceipts(List<Officialreceipts> officialreceipts)
        {
            if (officialreceipts != null && officialreceipts.Count != 0)
            {
                var body = new
                {
                    resource = officialreceipts
                };
                RestClient restClient = new RestClient(_receipts);
                RestRequest restRequest = new RestRequest("", Method.POST);
                restRequest.AddHeader("X-Application-Key", _apiKey);
                restRequest.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse restResponse = restClient.Execute(restRequest);
                Console.WriteLine($"{restResponse.Content}\n");
            }
        }
    }
}

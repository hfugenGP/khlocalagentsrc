using BillsExport.Models;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class PosthocBill
    {
        private FbDataReader _invoices;

        public PosthocBill(ref FbDataReader invoices)
        {
            _invoices = invoices;
            _buildInvoice();
        }

        public List<BillingInvoice> InvoiceLst
        {
            get;
            private set;
        } = new List<BillingInvoice>();

        private void _buildInvoice()
        {
            while(_invoices.Read())
            {
                object[] values = new object[_invoices.FieldCount];
                _invoices.GetValues(values);
                BillingInvoice item = new BillingInvoice();
                string[] unitInfo = _extractUnit(values[3]);
                item.reference_id = values[1].ToString();
                item.description = values[10].ToString();
                item.project_code = values[13].ToString();
                item.status = _billStatus(values[9], values[7], values[18]);
                item.due_at = Convert.ToDateTime(values[9].ToString()).ToString("yyyy-MM-dd");
                item.block = "Unit";
                item.floor = (unitInfo[0] != "" ? unitInfo[0] : "");
                item.unit = (unitInfo[1] != "" ? unitInfo[1] : "");
                item.currency = "RM";
                item.amount_cents = _amountToCents(values[17]);
                item.outstanding_amount_cents = _outStandingAmount(values[17], values[18]);
                item.reminder_days = int.Parse(values[8].ToString().Substring(0, values[8].ToString().IndexOf(@"Days")));
                item.type = _billType(values[13]);
                item.billing_date = Convert.ToDateTime(values[5].ToString()).ToString("yyyy-MM-dd");
                item.res_uuid = ConfigurationManager.AppSettings["uuid"];
                InvoiceLst.Add(item);
            }
        }

        private string[] _extractUnit(object code)
        {
            string[] unit = new string[2];
            if (code.ToString() != "")
            {
                unit = code.ToString().Substring(1, code.ToString().Length - 1).Split('-');
            }
            return unit;
        }

        private string _billStatus(object duedate, object amountdue, object paymentamt)
        {
            string status = "PENDING";
            float amount = float.Parse(amountdue.ToString());
            float payment = float.Parse(paymentamt.ToString());
            if (payment != 0 && (amount <= payment))
            {
                status = "PAID";
            }
            else
            {
                if (_billOverdue(DateTime.Parse(duedate.ToString())) == false)
                {
                    status = "OVERDUE";
                }
            }
            return status;
        }

        private bool _billOverdue(DateTime duedate)
        {
            bool overdue = false;
            int res = DateTime.Compare(DateTime.Now, duedate);
            if (res < 0)
            {
                overdue = true;
            }
            return overdue;
        }

        private int _amountToCents(object amount)
        {
            return (int)float.Parse(amount.ToString()) * 100;
        }

        private int _outStandingAmount(object amountdue, object amountpaid)
        {
            return _amountToCents(amountdue) - _amountToCents(amountpaid);
        }

        private string _billType(object project)
        {
            string type = "";
            switch(project.ToString())
            {
                case "WT": // WATER
                case "IWK": // INDAH WATER
                case "GAS": // GAS
                    type = "UTILITY";
                    break;
                default:
                    type = "MANAGEMENT";
                    break;
            }
            return type;
        }
    }
}

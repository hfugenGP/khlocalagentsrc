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
                item.status = _billStatus(values[9], values[17], values[18]);
                item.due_at = Convert.ToDateTime(values[9].ToString()).ToString("yyyy-MM-dd");

                /*
                 * ======================================
                 *  Use this unit format for Menara Geno
                 *  =====================================
                 */
                item.block = (_withInBounds(2, unitInfo) ? unitInfo[2] : "Unit");
                item.floor = (_withInBounds(0, unitInfo) ? unitInfo[0] : "");
                item.unit = (_withInBounds(1, unitInfo) ? unitInfo[1] : "");

                /*
                 * ======================================
                 *    Use this for all other clients.
                 * ======================================   
                 */

                /*
                item.block = (_withInBounds(0, unitInfo) ? unitInfo[0] : "Unit");
                item.floor = (_withInBounds(1, unitInfo) ? unitInfo[1] : "");
                item.unit = (_withInBounds(2, unitInfo) ? unitInfo[2] : "");
                */

                item.currency = "RM";
                item.amount_cents = _amountToCents(values[17]);
                item.outstanding_amount_cents = _outStandingAmount(values[17], values[18]);
                item.reminder_days = int.Parse(values[8].ToString().Substring(0, values[8].ToString().IndexOf(@"Days")));
                item.type = _billType(values[13]);
                item.billing_date = Convert.ToDateTime(values[5].ToString()).ToString("yyyy-MM-dd");
                item.residence_uuid = ConfigurationManager.AppSettings["uuid"];
                InvoiceLst.Add(item);
            }
        }

        private string[] _extractUnit(object code)
        {
            string[] unit = new string[3];
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
            float fl = float.Parse(amount.ToString()) * 100;
            int iFl = Convert.ToInt32(fl);
            // return (int)float.Parse(amount.ToString()) * 100;
            return iFl;
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

        /**
         * Check if array index is within 
         * the array length.
         * 
         */
        private bool _withInBounds<T>(int index, T[] array)
        {
            bool withIn = false; 
            if (index >= 0 && index < array.Length)
            {
                withIn = true;
            }
            return withIn;
        }
    }
}

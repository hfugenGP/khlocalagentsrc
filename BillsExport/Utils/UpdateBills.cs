using BillsExport.Models;
using BillsExport.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class UpdateBills
    {
        private List<OutstandingBill> _outStandingBills;
        private string _dbpath;
        private List<OutstandingBill> _updatedBills;

        public UpdateBills(List<OutstandingBill> outstandingBills, string dbpath)
        {
            _outStandingBills = outstandingBills;
            _dbpath = dbpath;
            _updatedBills = new List<OutstandingBill>();
            _checkPayment();
        }

        private void _checkPayment()
        {
            foreach(OutstandingBill bill in _outStandingBills)
            {
                Arpm arpm = _verifyPayment(bill.reference_id);
                if (!String.IsNullOrEmpty(arpm.DESCRIPTION))
                {
                    OutstandingBill outstandingBill = new OutstandingBill();
                    int balance = _adjustBalance(bill.outstanding_amount_cents, double.Parse(arpm.DOCAMT));
                    outstandingBill.uuid = bill.uuid;
                    outstandingBill.reference_id = bill.reference_id;
                    outstandingBill.amount_cents = bill.amount_cents;
                    outstandingBill.outstanding_amount_cents = balance;
                    outstandingBill.status = _setStatus(balance, bill.status);
                    _updatedBills.Add(outstandingBill);
                }
            }
        }

        /**
         * Check db table for payments made on reference id
         * 
         */
        private Arpm _verifyPayment(string refid)
        {
            Arpm arpm = new Arpm(_dbpath);
            arpm.payment(refid);
            return arpm;
        }

        private int _adjustBalance(int balance, double payment)
        {
            int outstanding = 0;
            outstanding = balance - (int)(payment * 10);
            return outstanding;
        }

        /**
         * Update status base on outstanding balance.
         * 
         */
        private string _setStatus(int outstanding, string status)
        {
            string current = status;
            if (outstanding <= 0)
            {
                current = "PAID";
            }
            return current;
        }

        public void exportPayment()
        {
            if (_updatedBills.Count != 0)
            {
                Console.WriteLine($"Payment count: {_updatedBills.Count}");
                _sendPayment(_updatedBills, 0);
            }
        }

        private void _sendPayment(List<OutstandingBill> payments, int index)
        {
            KipleHomeService kipleHome = new KipleHomeService();
            try
            {
                int batchSize = 10;
                List<OutstandingBill> currentBatch = Excerpt.extract(payments, index, index + batchSize);
                if (currentBatch.Count != 0)
                {
                    kipleHome.exportPayments(currentBatch);
                    _sendPayment(payments, index + batchSize);
                }
                else
                {
                    Console.WriteLine("No more payments to send.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

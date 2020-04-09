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
        private Arpm _arpm;
        private Ariv _ariv;

        public UpdateBills(List<OutstandingBill> outstandingBills, string dbpath)
        {
            _outStandingBills = outstandingBills;
            _dbpath = dbpath;
            _updatedBills = new List<OutstandingBill>();
            _arpm = new Arpm(_dbpath);
            _ariv = new Ariv(_dbpath);
            _checkPayment();
        }

        private void _checkPayment()
        {
            foreach(OutstandingBill bill in _outStandingBills)
            {
                _ariv.checkPayment(bill.reference_id);
                if (!String.IsNullOrEmpty(_ariv.DOCNO) && _ariv.PAYMENTAMT != 0)
                {
                    // Console.WriteLine(bill.reference_id + " " + _ariv.PAYMENTAMT); 
                    Console.WriteLine(_ariv.DOCNO + " " + _ariv.PAYMENTAMT);
                    OutstandingBill outstandingBill = new OutstandingBill();
                    int balance = _adjustBalance(_ariv.DOCAMT, _ariv.PAYMENTAMT);
                    outstandingBill.uuid = bill.uuid;
                    outstandingBill.reference_id = bill.reference_id;
                    outstandingBill.amount_cents = bill.amount_cents;
                    outstandingBill.outstanding_amount_cents = balance;
                    outstandingBill.status = _setStatus(balance, bill.status);
                    _updatedBills.Add(outstandingBill);
                }
                else
                {
                    Console.WriteLine($"No payment found for : {bill.reference_id}");
                }
            }
        }

        /**
         * Check db table for payments made on reference id
         * 
         */
        private Arpm _verifyPayment(string refid)
        {
            // Arpm arpm = new Arpm(_dbpath);
            _arpm.payment(refid);
            return _arpm;
        }

        private int _adjustBalance(double balance, double payment)
        {
            int outstanding = 0;
            outstanding = (int)(balance - payment) * 100;
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
                Console.WriteLine($"Payment count: {_updatedBills.Count} \n");
                _sendPayment(_updatedBills, 0);
            }
            else
            {
                Console.WriteLine($"No Payments found for outstanding bills. \n");
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
                    Console.WriteLine("Done exporting payments.\n");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("==========================================\n");
                Console.WriteLine("From Update:\n");
                Console.WriteLine(ex.Message);
                Console.WriteLine("==========================================\n");
            }
        }
    }
}

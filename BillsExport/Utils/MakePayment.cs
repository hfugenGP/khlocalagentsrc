using BillsExport.Models;
using BillsExport.Service;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsExport.Utils
{
    class MakePayment
    {
        private List<AppPayments> _adyenPayments;
        private string _dbpath;
        private Arpm _arpm;
        private Gltrans _gltrans;
        private List<Officialreceipts> _khReceipts;
       
        public MakePayment(List<AppPayments> adyenPayments, string dbpath)
        {
            _adyenPayments = adyenPayments;
            _dbpath = dbpath;
            _arpm = new Arpm(dbpath);
            _gltrans = new Gltrans(dbpath);
            _khReceipts = new List<Officialreceipts>();
        }

        public void insertPayments()
        {            

            foreach (AppPayments payment in _adyenPayments)
            {
                Console.WriteLine($"Checking payments for {payment.invoice_no} \n");
                Ariv ariv = _checkbyInvoiceno(payment.invoice_no);
                int gltransid = _gltrans.nextTransid();

                if (!String.IsNullOrEmpty(ariv.DOCNO))
                {
                    Console.WriteLine($"Recording the amount of {_convertAmount(payment.amount_paid)} for invoice : {payment.invoice_no}\n");

                    // _addGltrans(payment, "M", $"N{payment.code}", gltransid); 
                    _addGltrans(payment, "M", $"{payment.unit_name}", gltransid);
                    _addGltrans(payment, "S", "310-000", gltransid);

                    int paymentDockey = _addPayment(payment, $"{payment.unit_name}", gltransid);
                    _addKnockoff(payment, ariv.DOCKEY, paymentDockey);
                    Console.WriteLine($"{ariv.PAYMENTAMT} + {_convertAmount(payment.amount_paid)}");
                    ariv.PAYMENTAMT = ariv.PAYMENTAMT + _convertAmount(payment.amount_paid);
                    ariv.Update(ariv.DOCNO);

                    // _adjustCustomerOutstanding(payment.amount_paid, $"N{payment.code}"); 
                    _adjustCustomerOutstanding(payment.amount_paid, $"{payment.unit_name}");

                    Console.WriteLine($"Updated payment : {ariv.PAYMENTAMT}\n");
                }
                else
                {
                    Console.WriteLine($"{payment.invoice_no} not found.\n");
                }
                
            }

            // Export receipts
            if (_khReceipts.Count != 0)
            {
                _exportReceipts(_khReceipts);
            }
        }

        private int _addPayment(AppPayments payment, string code, int gltransid)
        {            
            Arpm arpm = new Arpm(_dbpath);
            Officialreceipts oreceipts = new Officialreceipts();

            // Get next Official receipt number
            Sysdocnodtl sysdocnodtl = new Sysdocnodtl(_dbpath);
            sysdocnodtl.nxtOffclrcpt();

            int dockey = arpm.nextDockey();
            string offclrcpt = $"OR-{sysdocnodtl.NEXTNUMBER.ToString().PadLeft(5,'0')}"; 

            arpm.DOCKEY = dockey;
            arpm.CODE = $"'{code}'";
            // arpm.DOCNO = $"'{arpm.nextOR()}'"; 
            arpm.DOCNO = $"'{offclrcpt}'";
            arpm.GLTRANSID = gltransid;
            arpm.DOCDATE = $"'{payment.billing_date}'";
            arpm.POSTDATE = $"'{payment.billing_date}'";
            arpm.TAXDATE = $"'{payment.billing_date}'";
            arpm.DESCRIPTION = $"'{payment.description}'";
            arpm.AREA = $"'{code.Substring(0, code.IndexOf("-"))}'";
            arpm.AGENT = "'----'";
            arpm.CANCELLED = "'F'"; 
            arpm.PROJECT = $"'{payment.project_code}'";
            arpm.PAYMENTMETHOD = "'310-000'";
            arpm.JOURNAL = "'BANK'";
            arpm.CURRENCYRATE = 1;
            arpm.BANKCHARGE = 0;
            arpm.DOCAMT = _convertAmount(payment.amount_paid);
            arpm.LOCALDOCAMT = _convertAmount(payment.amount_paid);
            arpm.UNAPPLIEDAMT = 0;
            arpm.GLTRANSID = gltransid;
            arpm.CHEQUENUMBER = "'KIPLEHOME - ONLINE'";
            arpm.UDF_POSTDN = "'F'";
            arpm.UDF_TAXRATE = "'0%'";
            arpm.Add();

            // Increment for next official reciept
            sysdocnodtl.NEXTNUMBER+= 1;
            sysdocnodtl.Update(sysdocnodtl.AUTOKEY);

            oreceipts.adyen_uuid = payment.uuid;
            oreceipts.or_no = offclrcpt;
            _khReceipts.Add(oreceipts);

            _addGlcb(payment, offclrcpt, gltransid, code);

            return dockey;
        }

        private float _convertAmount(int amount)
        {
            float converted = 0;
            if (amount > 0)
            {
                //converted = (amount / 100);
                converted = (float)Decimal.Divide(amount, 100);
            }
            return converted;
        } 

        private void _addGltrans(AppPayments payment, string tabletype, string code, int gltransid)
        {
            Gltrans gLtrans = new Gltrans(_dbpath);
            gLtrans.DOCKEY = gLtrans.nextDockey();
            gLtrans.GLTRANSID = gltransid;
            if (tabletype == "M")
            {
                Arcustomer arcustomer = new Arcustomer(_dbpath);
                arcustomer.findBycode(code);
                gLtrans.DESCRIPTION = $"'{arcustomer.COMPANYNAME}'";
            }
            else
            {
                gLtrans.DESCRIPTION2 = $"'{payment.description}'";
            }
            gLtrans.CODE = $"'{code}'";
            gLtrans.JOURNAL = "'BANK'";
            gLtrans.FROMDOCTYPE = "'OR'";
            gLtrans.DOCDATE = $"'{payment.billing_date}'";
            gLtrans.POSTDATE = $"'{payment.billing_date}'";
            gLtrans.TAXDATE = $"'{payment.billing_date}'";
            gLtrans.REF1 = $"'{payment.invoice_no}'";
            gLtrans.TABLETYPE = $"'{tabletype}'";
            gLtrans.CANCELLED = "'F'";
            if (tabletype == "M")
            {
                gLtrans.DR = _convertAmount(payment.amount_paid);
                gLtrans.LOCALDR = _convertAmount(payment.amount_paid);
            }
            else
            {
                gLtrans.CR = _convertAmount(payment.amount_paid);
                gLtrans.LOCALCR = _convertAmount(payment.amount_paid);
            }
            gLtrans.CURRENCYRATE = 1;
            gLtrans.FROMKEY = gLtrans.nextFromkey();
            gLtrans.Add();
        }

        private void _addKnockoff(AppPayments payment, int dockey, int paymentdockey)
        {
            Apknockoff apknockoff = new Apknockoff(_dbpath);
            apknockoff.DOCKEY = apknockoff.nextDockey();
            apknockoff.FROMDOCTYPE = "'PM'";
            apknockoff.FROMDOCKEY = paymentdockey;
            apknockoff.TODOCTYPE = "'IV'";
            apknockoff.TODOCKEY = dockey;
            apknockoff.KOAMT = _convertAmount(payment.amount_paid);
            apknockoff.ACTUALLOCALKOAMT = _convertAmount(payment.amount_paid);
            apknockoff.LOCALKOAMT = _convertAmount(payment.amount_paid);
            apknockoff.KOTAXDATE = $"'{payment.billing_date}'";
            apknockoff.GAINLOSS = 0;
            apknockoff.Add();
        } 

        private void _adjustCustomerOutstanding(int payment, string customercode)
        {
            Arcustomer arcustomer = new Arcustomer(_dbpath);
            arcustomer.findBycode(customercode);
            arcustomer.OUTSTANDING = (arcustomer.OUTSTANDING - _convertAmount(payment));
            arcustomer.Update(customercode);
        }

        private Ariv _checkbyInvoiceno(string invoiceno)
        {
            Ariv ariv = new Ariv(_dbpath);
            ariv.checkPayment(invoiceno);
            return ariv;
        }

        private void _exportReceipts(List<Officialreceipts> officialreceipts)
        {
            KipleHomeService kipleApi = new KipleHomeService();
            Console.WriteLine("Exporting receipts to kiple home...\n");
            kipleApi.exportReceipts(officialreceipts);
        } 

        private void _addGlcb(AppPayments payment,string ornumber, int gltransid, string code)
        {
            Glcb glCb = new Glcb(_dbpath);
            glCb.DOCKEY = glCb.nextDockey();
            glCb.DOCNO = $"'{ornumber}'";
            glCb.DOCTYPE = "'OR'";
            glCb.DOCDATE = $"'{payment.billing_date}'";
            glCb.POSTDATE = $"'{payment.billing_date}'";
            glCb.TAXDATE = $"'{payment.billing_date}'";
            glCb.DESCRIPTION = $"'{payment.description}'";
            glCb.PAYMENTMETHOD = "'310-000'";
            glCb.JOURNAL = "'CASH'";
            glCb.CHEQUENUMBER = "'KIPLEHOME - ONLINE'";
            glCb.DOCAMT = _convertAmount(payment.amount_paid);
            glCb.LOCALDOCAMT = _convertAmount(payment.amount_paid);
            glCb.GLTRANSID = gltransid;
            glCb.CANCELLED = "'F'"; 
            glCb.AREA = $"'{code.Substring(0, code.IndexOf("-"))}'"; 
            glCb.AGENT = "'----'";
            glCb.PROJECT = $"'{payment.project_code}'";
            glCb.Add();
            _addGlcbdtl(payment, glCb.DOCKEY, code);
        }

        private void _addGlcbdtl(AppPayments payment, int glcbdockkey, string code)
        {
            Glcbdtl glcbdtl = new Glcbdtl(_dbpath);
            glcbdtl.DTLKEY = glcbdtl.nextDTLKEY();
            glcbdtl.DOCKEY = glcbdockkey;
            glcbdtl.SEQ = 1;
            glcbdtl.CODE = $"'{code}'";
            glcbdtl.DESCRIPTION = $"'{payment.description}'";
            glcbdtl.AMOUNT = _convertAmount(payment.amount_paid);
            glcbdtl.LOCALAMOUNT = _convertAmount(payment.amount_paid);
            glcbdtl.CURRENCYAMOUNT = _convertAmount(payment.amount_paid);
            glcbdtl.Add();
        }
    }
}

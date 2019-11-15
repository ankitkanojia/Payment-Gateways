using PaymentGateways.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace PaymentGateways.Controllers
{
    public class CyberSourceController : Controller
    {
        public ActionResult Index(CyberSourceVM data)
        {
            /*Note: -- THIS IS MOST IMPORTANT BEFORE START CODE
          You need to setup setting in your CYBER SOURCE account else nothing works.
          You need to specufy return URL in your CYBER SOURCE account
          You need to check all parameter in DICTIONARY object and CLASS object is same in "DoCyberSorcePayment" method else you got unauthorise error
          */

            data.TrasactionID = Guid.NewGuid().ToString().Substring(10);

            return RedirectToAction("DoCyberSorcePayment", "PaymentGateway", data);
        }

        [HttpGet]
        public ActionResult DoCyberSorcePayment(CyberSourceVM data)
        {
            try
            {
                //For Creating A Signature
                Dictionary<string, string> paramsArray = new Dictionary<string, string>();

                //Generate Some Basic Variable with Unique Value
                string uuid = Convert.ToString(Guid.NewGuid());
                string signDT = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                string TransactionID = data.TrasactionID;
                string signature = string.Empty;

                //Get this information from USER
                CyberSourceDetailVM cyberSourceDetailVM = new CyberSourceDetailVM();
                cyberSourceDetailVM.amount = 1000;
                cyberSourceDetailVM.bill_to_firstname = "ankit";
                cyberSourceDetailVM.bill_to_surname = "kanojia";
                cyberSourceDetailVM.bill_to_email = "ankitkanojia.rs@gmail.com";
                cyberSourceDetailVM.bill_to_phone = "9099673090";
                cyberSourceDetailVM.bill_to_address_line1 = "58, Trishul tenament";
                cyberSourceDetailVM.bill_to_address_city = "bharuch";
                cyberSourceDetailVM.bill_to_address_state = "CA";
                cyberSourceDetailVM.bill_to_address_country = "in";
                cyberSourceDetailVM.bill_to_address_postal_code = "392012";

                paramsArray.Add("access_key", ConfigurationManager.AppSettings["AccessKey"].ToString());
                paramsArray.Add("profile_id", ConfigurationManager.AppSettings["ProfileID"].ToString());
                paramsArray.Add("transaction_uuid", uuid);
                paramsArray.Add("signed_field_names", "access_key,profile_id,transaction_uuid,signed_field_names,unsigned_field_names,signed_date_time,locale,transaction_type,reference_number,amount,currency,payment_method,bill_to_forename,bill_to_surname,bill_to_email,bill_to_phone,bill_to_address_line1,bill_to_address_city,bill_to_address_state,bill_to_address_country,bill_to_address_postal_code");
                paramsArray.Add("unsigned_field_names", "card_type,card_number,card_expiry_date");
                paramsArray.Add("signed_date_time", signDT);
                paramsArray.Add("locale", "en");
                paramsArray.Add("transaction_type", "authorization");
                paramsArray.Add("reference_number", TransactionID);
                paramsArray.Add("amount", Convert.ToString(cyberSourceDetailVM.amount));
                paramsArray.Add("currency", "ZMW");
                paramsArray.Add("payment_method", "card");
                paramsArray.Add("bill_to_forename", cyberSourceDetailVM.bill_to_firstname.Trim());
                paramsArray.Add("bill_to_surname", cyberSourceDetailVM.bill_to_surname.Trim());
                paramsArray.Add("bill_to_email", cyberSourceDetailVM.bill_to_email.Trim());
                paramsArray.Add("bill_to_phone", cyberSourceDetailVM.bill_to_phone.Trim());
                paramsArray.Add("bill_to_address_line1", cyberSourceDetailVM.bill_to_address_line1.Trim());
                paramsArray.Add("bill_to_address_city", cyberSourceDetailVM.bill_to_address_city.Trim());
                paramsArray.Add("bill_to_address_state", cyberSourceDetailVM.bill_to_address_state.Trim());
                paramsArray.Add("bill_to_address_country", cyberSourceDetailVM.bill_to_address_country.Trim());
                paramsArray.Add("bill_to_address_postal_code", cyberSourceDetailVM.bill_to_address_postal_code.Trim());
                paramsArray.Add("submit", "Submit");
                signature = Security.sign(paramsArray);

                CyberSourceReqVM cyberSourceReqVM = new CyberSourceReqVM();
                cyberSourceReqVM.access_key = ConfigurationManager.AppSettings["AccessKey"].ToString();
                cyberSourceReqVM.profile_id = ConfigurationManager.AppSettings["ProfileID"].ToString();
                cyberSourceReqVM.transaction_uuid = uuid;
                cyberSourceReqVM.signed_field_names = "access_key,profile_id,transaction_uuid,signed_field_names,unsigned_field_names,signed_date_time,locale,transaction_type,reference_number,amount,currency,payment_method,bill_to_forename,bill_to_surname,bill_to_email,bill_to_phone,bill_to_address_line1,bill_to_address_city,bill_to_address_state,bill_to_address_country,bill_to_address_postal_code";
                cyberSourceReqVM.unsigned_field_names = "card_type,card_number,card_expiry_date";
                cyberSourceReqVM.signed_date_time = signDT;
                cyberSourceReqVM.locale = "en";
                cyberSourceReqVM.transaction_type = "authorization";
                cyberSourceReqVM.reference_number = TransactionID;
                cyberSourceReqVM.amount = Convert.ToString(cyberSourceDetailVM.amount);
                cyberSourceReqVM.currency = "ZMW";
                cyberSourceReqVM.payment_method = "card";
                cyberSourceReqVM.bill_to_forename = cyberSourceDetailVM.bill_to_firstname.Trim();
                cyberSourceReqVM.bill_to_surname = cyberSourceDetailVM.bill_to_surname.Trim();
                cyberSourceReqVM.bill_to_email = cyberSourceDetailVM.bill_to_email.Trim();
                cyberSourceReqVM.bill_to_phone = cyberSourceDetailVM.bill_to_phone.Trim();
                cyberSourceReqVM.bill_to_address_line1 = cyberSourceDetailVM.bill_to_address_line1.Trim();
                cyberSourceReqVM.bill_to_address_city = cyberSourceDetailVM.bill_to_address_city.Trim();
                cyberSourceReqVM.bill_to_address_postal_code = cyberSourceDetailVM.bill_to_address_postal_code.Trim();
                cyberSourceReqVM.bill_to_address_state = cyberSourceDetailVM.bill_to_address_state.Trim();
                cyberSourceReqVM.bill_to_address_country = cyberSourceDetailVM.bill_to_address_country.Trim();
                cyberSourceReqVM.card_type = data.VisaType;
                cyberSourceReqVM.card_number = data.CardNumber;
                cyberSourceReqVM.card_expiry_date = data.Month + "-" + data.Year;
                cyberSourceReqVM.signature = signature;
                cyberSourceReqVM.submit = "Submit";

                return View(cyberSourceReqVM);
            }
            catch
            {
                throw;
            }
        }

        public ActionResult CyberSourceReturnURL(CyberSourceResVM data)
        {
            return View();
        }
    }
}
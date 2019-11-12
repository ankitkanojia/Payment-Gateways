using PaymentGateways.Helpers;
using PaymentGateways.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentGateways.Controllers
{
    public class PayUMoneyController : Controller
    {
        [HttpGet]
        public ActionResult PayUMoney()
        {
            try
            {
                PayuRequest payuRequest = new PayuRequest();
                payuRequest.firstName = "Ankit";
                payuRequest.lastname = "kanojia";
                payuRequest.amount = "100.00";
                payuRequest.productInfo = "Moto G4 Plus";
                payuRequest.email = "ankitkanojia.rs@gmail.com";
                payuRequest.phone = "9099673090";
                payuRequest.address1 = "O/P Inox multiplex";
                payuRequest.address2 = "Zadeshwar Road";
                payuRequest.city = "BHaruch";
                payuRequest.state = "Gujarat";
                payuRequest.zipcode = "392012";
                return View(payuRequest);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void PayUMoney(PayuRequest data)
        {
            try
            {
                RemotePost myremotepost = new RemotePost();
                string key = ConfigurationManager.AppSettings["MERCHANT_KEY"];
                string salt = ConfigurationManager.AppSettings["SALT"];
                string surl = ConfigurationManager.AppSettings["SUCC_URL"];
                string furl = ConfigurationManager.AppSettings["FAIL_URL"];

                //posting all the parameters required for integration.
                myremotepost.Url = ConfigurationManager.AppSettings["PAYU_BASE_URL"];
                myremotepost.Add("key", key);
                string txnid = Guid.NewGuid().ToString().Substring(12);
                myremotepost.Add("txnid", txnid);
                myremotepost.Add("amount", data.amount);
                myremotepost.Add("productinfo", data.productInfo);
                myremotepost.Add("firstName", data.firstName);
                myremotepost.Add("lastname", data.lastname);
                myremotepost.Add("email", data.email);
                myremotepost.Add("phone", data.phone);
                myremotepost.Add("address1", data.address1);
                myremotepost.Add("address2", data.address2);
                myremotepost.Add("city", data.city);
                myremotepost.Add("state", data.state);
                myremotepost.Add("zipcode", data.zipcode);
                myremotepost.Add("surl", surl);
                myremotepost.Add("furl", furl);
                string hashString = key + "|" + txnid + "|" + data.amount + "|" + data.productInfo + "|" + data.firstName + "|" + data.email + "|||||||||||" + salt;
                string hash = Hashing.Generatehash512(hashString);
                myremotepost.Add("hash", hash);
                myremotepost.Post();
            }
            catch
            {
                throw;
            }
        }
    }
}
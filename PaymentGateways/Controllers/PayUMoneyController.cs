using PaymentGateways.Models;
using System;
using System.Collections.Generic;
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
    }
}
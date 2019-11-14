using PaymentGateways.Models;
using System;
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
    }
}
using System;
using System.Web.Configuration;
using System.Web.Mvc;
using Twilio;

namespace PaymentGateways.Controllers
{
    public class TwillioController : Controller
    {
        public ActionResult TwillioSMS()
        {
            /*Need to install below library
              Install-Package Twilio
            */

            try
            {
                // Find your Account Sid and Auth Token at twilio.com/user/account
                string AccountSid = WebConfigurationManager.AppSettings["Twillio_AccountSid"].ToString();
                string AuthToken = WebConfigurationManager.AppSettings["Twillio_AuthToken"].ToString();

                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                //Send SMS
                var message = twilio.SendMessage("+919099673090", "+919879322283", "This is test SMS");
            }
            catch (Exception e)
            {
                throw;
            }

            return View();
        }

        public ActionResult TwillioVoiceCall()
        {
            /*Need to install below library
             Install-Package Twilio
           */
            try
            {
                // Find your Account Sid and Auth Token at twilio.com/user/account
                string AccountSid = WebConfigurationManager.AppSettings["Twillio_AccountSid"].ToString();
                string AuthToken = WebConfigurationManager.AppSettings["Twillio_AuthToken"].ToString();

                var twilio = new TwilioRestClient(AccountSid, AuthToken);

                //Send SMS
                var message = twilio.SendMessage("+919099673090", "+919879322283", "This is test SMS");

                //Make Voice Call
                var options = new CallOptions();
                options.Url = "http://demo.twilio.com/docs/voice.xml";
                options.To = "+919099673090";
                options.From = "+919879322283";
                var call = twilio.InitiateOutboundCall(options);

            }
            catch (Exception e)
            {
                throw;
            }

            return View();
        }

    }
}
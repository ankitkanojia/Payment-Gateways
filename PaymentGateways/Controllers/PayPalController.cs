using System;
using System.Collections.Generic;
using PayPal.Api;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PaymentGateways.Controllers
{
    public class PayPalController : Controller
    {
        private PayPal.Api.Payment payment;

        public ActionResult Index()
        {
            /*Need below Library
            Install-package paypal
            Install-package log4net
            */
            //getting the apiContext as earlier
            APIContext apiContext = Models.Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PaymentGateway/PayPal?";

                    //guid we are generating for storing the paymentID received in session               
                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url on which payer is redirected for paypal acccount payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // Executing a payment
                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    var json = new JavaScriptSerializer().Serialize(executedPayment);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("PaypalFail");
                    }

                }
            }
            catch (Exception ex)
            {
                return View("PaypalException");
            }

            return View("PaypalSuccess");
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };


            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = "6",

            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Recharge Amount. RS. 500",
                invoice_number = Guid.NewGuid().ToString().Substring(10),
                amount = amount,
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
    }
}
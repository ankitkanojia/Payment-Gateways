using PaymentGateways.Models;
using RestSharp;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace PaymentGateways.Controllers
{
    public class IDTController : Controller
    {
        public ActionResult Index()
        {
            IDTMobileTopUp[] xData = null;

            try
            {
                #region --> Fetch Mobile Topup Plan
                //Request to IDT WEB API
                var client = new RestClient(WebConfigurationManager.AppSettings["Fetchdata_URL"].ToString());
                var request = new RestRequest(Method.GET);

                //Add Header in Request
                request.AddHeader("x-idt-beyond-app-key", WebConfigurationManager.AppSettings["header_key_Sandbox"].ToString());
                request.AddHeader("x-idt-beyond-app-id", WebConfigurationManager.AppSettings["header_id_Sandbox"].ToString());

                //Get Response from IDT WEB API
                IRestResponse response = client.Execute(request);

                //Convert JSON to ARRAY 
                JavaScriptSerializer js = new JavaScriptSerializer();
                xData = js.Deserialize<IDTMobileTopUp[]>(response.Content);
                #endregion

                #region --> API CALL for Topup Mobile 

                //Required Object
                string countryCode = "91";
                string topupCode = "10";
                string MobileNo = "9099673090";
                string amount = "100";
                string ClientTransactionID = "001";

                //Request to IDT for Topup
                string JSONpost = "{\"country_code\":\"" + countryCode + "\",\"carrier_code\":\"" + topupCode + "\",\"mobile_number\":" + MobileNo + ",\"plan\":\"Production\",\"amount\":" + amount + ",\"client_transaction_id\":\"" + ClientTransactionID + "\",\"terminal_id\": \"Kiosk 5\",\"product_code\":\"\"}";
                //string JSONpost = "{\"country_code\":\"SV\",\"carrier_code\":\"Claro\",\"mobile_number\":50322222218,\"plan\":\"Sandbox\",\"amount\":500,\"client_transaction_id\":\"" + ClientTransactionID + "\",\"terminal_id\": \"Kiosk 5\",\"product_code\":\"\"}";
                client = new RestClient(WebConfigurationManager.AppSettings["Topup_URL"].ToString());
                request = new RestRequest(Method.POST);

                //Add Header & Parameter in Request
                request.AddHeader("x-idt-beyond-app-key", WebConfigurationManager.AppSettings["header_key_Sandbox"].ToString());
                request.AddHeader("x-idt-beyond-app-id", WebConfigurationManager.AppSettings["header_id_Sandbox"].ToString());
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", JSONpost, ParameterType.RequestBody);
                response = client.Execute(request);

                //Filter Responce Code & Messge from Response
                js = new JavaScriptSerializer();
                TopUpPostResponse responseJson = js.Deserialize<TopUpPostResponse>(response.Content);
                #endregion
            }
            catch
            {
                throw;
            }

            return View();
        }
    }
}
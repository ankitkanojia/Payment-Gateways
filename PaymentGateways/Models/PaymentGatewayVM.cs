using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace PaymentGateways.Models
{
    public class TopUpPostResponse
    {
        public bool success { get; set; }
        public string transaction_id { get; set; }
        public string client_transaction_id { get; set; }
        public List<TopUpPostResponseMessage> messages { get; set; }
    }

    public class TopUpPostResponseMessage
    {
        public string status_code { get; set; }
        public string message { get; set; }
    }

    public class IDTMobileTopUp
    {
        public string display_name { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string carrier { get; set; }
        public string carrier_code { get; set; }
        public string mask { get; set; }
        public string denomination { get; set; }
        public string min_denomination { get; set; }
        public string max_denomination { get; set; }
        public string product_code { get; set; }
        public string commission { get; set; }
    }

    public static class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Static constructor for setting the readonly static members.
        static Configuration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext()
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }

    public class CyberSourceVM
    {
        public string VisaType { get; set; }

        public string CardNumber { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string TrasactionID { get; set; }
    }


    public class CyberSourceDetailVM
    {
        public string accesskey { get; set; }
        public string merchant_id { get; set; }
        public string reference_no { get; set; }
        public decimal amount { get; set; }
        public string bill_to_firstname { get; set; }
        public string bill_to_surname { get; set; }
        public string bill_to_email { get; set; }
        public string bill_to_phone { get; set; }
        public string bill_to_address_line1 { get; set; }
        public string bill_to_address_city { get; set; }
        public string bill_to_address_postal_code { get; set; }
        public string bill_to_address_state { get; set; }
        public string bill_to_address_country { get; set; }
        public string return_url { get; set; }
        public string ipaddress { get; set; }
        public string browser { get; set; }
        public Nullable<bool> is_mobile { get; set; }
        public string request_url { get; set; }
        public System.DateTime request_date_time { get; set; }
        public Nullable<long> payment_method_id { get; set; }
        public long service_detail_id { get; set; }
        public long transaction_charge_id { get; set; }
        public long progress_id { get; set; }
    }

    public static class Security
    {

        public static String SECRET_KEY = "eb03958467424319b22adf747364f83e62f354a4a7174a97890071fa30b84f17b418e7d27ceb478ca7ef8e3296f495af077598929d594ef1846f2085fb072e684c97ad8613e0447a92cbdb8f1b7d477ab74b7cd9fc3e45a3a58fccf2051c750107cd1fcf5d564671a3baa3c053457834b0d63729692345fea64f90b174316bfc";
        public static String sign(IDictionary<string, string> paramsArray)
        {
            return sign(buildDataToSign(paramsArray), SECRET_KEY);
        }

        private static String sign(String data, String secretKey)
        {
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
        }

        private static String buildDataToSign(IDictionary<string, string> paramsArray)
        {
            String[] signedFieldNames = paramsArray["signed_field_names"].Split(',');
            IList<string> dataToSign = new List<string>();

            foreach (String signedFieldName in signedFieldNames)
            {
                dataToSign.Add(signedFieldName + "=" + paramsArray[signedFieldName]);
            }

            return commaSeparate(dataToSign);
        }

        private static String commaSeparate(IList<string> dataToSign)
        {
            return String.Join(",", dataToSign);
        }
    }

    public class CyberSourceReqVM
    {
        public string access_key { get; set; }

        public string profile_id { get; set; }

        public string transaction_uuid { get; set; }

        public string signed_field_names { get; set; }

        public string unsigned_field_names { get; set; }

        public string signed_date_time { get; set; }

        public string locale { get; set; }

        public string transaction_type { get; set; }

        public string reference_number { get; set; }

        public string amount { get; set; }

        public string currency { get; set; }

        public string payment_method { get; set; }

        public string bill_to_forename { get; set; }

        public string bill_to_surname { get; set; }

        public string bill_to_email { get; set; }

        public string bill_to_phone { get; set; }

        public string bill_to_address_line1 { get; set; }

        public string bill_to_address_city { get; set; }

        public string bill_to_address_postal_code { get; set; }

        public string bill_to_address_state { get; set; }

        public string bill_to_address_country { get; set; }

        public string card_type { get; set; }

        public string card_number { get; set; }

        public string card_expiry_date { get; set; }

        public string signature { get; set; }

        public string submit { get; set; }
    }

    public class CyberSourceResVM
    {
        public string req_bill_to_forename { get; set; }

        public string req_bill_to_surname { get; set; }

        public string req_bill_to_email { get; set; }

        public string req_bill_to_address_line1 { get; set; }

        public string req_bill_to_address_city { get; set; }

        public string req_bill_to_address_postal_code { get; set; }

        public string req_bill_to_address_state { get; set; }

        public string req_bill_to_address_country { get; set; }

        public string unsigned_field_names { get; set; }

        public bool isSuccess { get; set; }

        public string ZemullaID { get; set; }

        public int payment_method_id { get; set; }

        public string req_reference_number { get; set; }

        public string req_transaction_type { get; set; }

        public string req_locale { get; set; }

        public string req_payment_method { get; set; }

        public string req_card_type { get; set; }

        public string req_card_number { get; set; }

        public string req_access_key { get; set; }

        public string req_profile_id { get; set; }

        public string req_transaction_uuid { get; set; }

        public string signed_date_time { get; set; }

        public string signed_field_names { get; set; }

        public string signature { get; set; }

        public string decision { get; set; }

        public string reason_code { get; set; }

        public string transaction_id { get; set; }

        public string req_payment_token { get; set; }

        public string message { get; set; }

        public string req_amount { get; set; }

        public string req_tax_amount { get; set; }

        public string req_currency { get; set; }
    }

    public class PayuRequest
    {
        public string firstName { get; set; }
        public string lastname { get; set; }
        public string amount { get; set; }
        public string productInfo { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
    }


    public class PayuResponse
    {
        public string mihpayid { get; set; }

        public string mode { get; set; }

        public string status { get; set; }

        public string txnid { get; set; }

        public string Error { get; set; }

        public string key { get; set; }

        public string amount { get; set; }

        public string discount { get; set; }

        public string offer { get; set; }

        public string productinfo { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string country { get; set; }

        public string zipcode { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string udf1 { get; set; }

        public string udf2 { get; set; }

        public string udf3 { get; set; }

        public string udf4 { get; set; }

        public string udf5 { get; set; }

        public string Hash { get; set; }

        public string Errorbankcode { get; set; }

        public string PG_TYPE { get; set; }

        public string bank_ref_num { get; set; }

        public string shipping_firstname { get; set; }

        public string shipping_lastname { get; set; }

        public string shipping_address1 { get; set; }

        public string shipping_address2 { get; set; }

        public string shipping_city { get; set; }

        public string shipping_state { get; set; }

        public string shipping_country { get; set; }

        public string shipping_zipcode { get; set; }

        public string shipping_phone { get; set; }

        public string unmappedstatus { get; set; }

        public bool IsSuccess { get; set; }

    }
}
using RestSharp;
using System;

namespace orderservice.Client
{
    public class VoucherClient
    {
        public static string RedeeemVoucher(string voucher)
        {
            var voucherclient = new RestClient("http://localhost:5004");
            var voucherrequest = new RestRequest("voucher/redeemvoucher/{voucher}", Method.GET);
            voucherrequest.AddUrlSegment("voucher", voucher);
            voucherrequest.Timeout = 2000;
            IRestResponse response = voucherclient.Execute(voucherrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Voucher service failed");
            var content = response.Content;
            return content;
        }

        public static string UnRedeeemVoucher(string voucher)
        {
            var voucherclient = new RestClient("http://localhost:5004");
            var voucherrequest = new RestRequest("voucher/unredeemvoucher/{voucher}", Method.GET);
            voucherrequest.AddParameter("voucher", voucher);
            voucherrequest.Timeout = 2;
            IRestResponse response = voucherclient.Execute(voucherrequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Voucher service failed");
            var content = response.Content;
            return content;
        }
    }
}

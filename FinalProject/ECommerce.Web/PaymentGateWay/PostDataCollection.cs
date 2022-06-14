using System.Collections.Specialized;

namespace ECommerce.Web.PaymentGateWay
{
    public static class PostDataCollection
    {
        public static NameValueCollection PostData(double totalPrice,string baseUrl
            ,string confirmUrl, string failedUrl, string cencelUrl, int storeId) 
        {
            NameValueCollection data = new NameValueCollection();
            data.Add("total_amount", $"{totalPrice}");
            string tran_id = Guid.NewGuid().ToString();
            data.Add("tran_id", tran_id);
            data.Add("success_url", $"{baseUrl}{confirmUrl}" +
                $"?tran_id={tran_id}&totalPrice={totalPrice}&storeId={storeId}");
            data.Add("fail_url", baseUrl + failedUrl);
            data.Add("cancel_url", baseUrl + cencelUrl);

            data.Add("version", "3.00");
            data.Add("cus_name", "ABC XY");
            data.Add("cus_email", "abc.xyz@mail.co");
            data.Add("cus_add1", "Address Line On");
            data.Add("cus_add2", "Address Line Tw");
            data.Add("cus_city", "City Nam");
            data.Add("cus_state", "State Nam");
            data.Add("cus_postcode", "Post Cod");
            data.Add("cus_country", "Countr");
            data.Add("cus_phone", "0111111111");
            data.Add("cus_fax", "0171111111");
            data.Add("ship_name", "ABC XY");
            data.Add("ship_add1", "Address Line On");
            data.Add("ship_add2", "Address Line Tw");
            data.Add("ship_city", "City Nam");
            data.Add("ship_state", "State Nam");
            data.Add("ship_postcode", "Post Cod");
            data.Add("ship_country", "Countr");
            data.Add("value_a", "ref00");
            data.Add("value_b", "ref00");
            data.Add("value_c", "ref00");
            data.Add("value_d", "ref00");
            data.Add("shipping_method", "NO");
            data.Add("num_of_item", "1");
            data.Add("product_name", $"Demo");
            data.Add("product_profile", "general");
            data.Add("product_category", "Demo");

            return data;
        }
    }
}

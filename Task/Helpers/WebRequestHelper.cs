using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace Task.Helpers
{
    public class WebRequestHelper
    {
        //public bool SendPostRequest(string url, object body)
        //{
        //    string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        //    using (var client = new HttpClient())
        //    {
        //        string targetUrl = baseUrl + "/api/Product";
        //        var request = new HttpRequestMessage(HttpMethod.Post, targetUrl);
        //        if (body != null)
        //            request.Content = 
        //        var result = client.SendAsync(request).Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //    }
        //    return true;
        //}
        public async Task<bool> SendPutRequest(object body)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                string targetUrl = "/api/Product";
                var result = await client.PutAsync(targetUrl, body, new JsonMediaTypeFormatter());
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
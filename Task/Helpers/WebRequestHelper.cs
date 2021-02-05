using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Task.Models.DTO_MVC;

namespace Task.Helpers
{
    public class WebRequestHelper
    {
        public async Task<List<ProductDTO>> SendGetRequest()
        {
            List<ProductDTO> ListProduct = null;
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/Product");

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ListProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(ProdResponse);
                }
                return ListProduct;
            }
        }

        public async Task<ProductDTO> SendGetRequest(int id)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            ProductDTO ProdInfo = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<ProductDTO>(ProdResponse);
                }
            }
            return ProdInfo;
        }
        
        public async Task<bool> SendPostRequest(ProductDTO Body)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var result = await client.PostAsync("/api/Product", Body, new JsonMediaTypeFormatter());
                if (result.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public async Task<bool> SendPutRequest(object body)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                string targetUrl = "/api/Product";
                var result = await client.PutAsync(targetUrl, body, new JsonMediaTypeFormatter());
                if (result.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }
    
       public async Task<bool> SendDelteRequest(int id)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.DeleteAsync("/api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public async Task<List<ProductDTO>> SendGetRequest(string Name)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            List<ProductDTO> ProdInfo = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("/api/Product?Name=" + Name);

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<List<ProductDTO>>(ProdResponse);
                }
            }
            return ProdInfo;
        }
    }
}
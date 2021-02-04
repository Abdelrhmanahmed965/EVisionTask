using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Task.Business;
using Task.Helpers;
using Task.Models.DTO_MVC;

namespace Task.Controllers.MVC
{
    public class ProductController : Controller
    {
        string Baseurl = "http://localhost:65378/";

        public async Task<ActionResult> Index()
        {
            List<ProductDTO> ProdInfo = new List<ProductDTO>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Product");

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<List<ProductDTO>>(ProdResponse);
                }

                return View(ProdInfo);
            }
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductDTO ProdInfo = new ProductDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<ProductDTO>(ProdResponse);
                }
            }

            if (ProdInfo == null)
            {
                return HttpNotFound();
            }
            return View(ProdInfo);
        }

        #region Create Product

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name,Photo,Price")] ProductDTO NewProduct, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    string path1 = Server.MapPath("~/Content/Image");
                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(path1);
                    }
                    var NewPhoto = Guid.NewGuid().ToString() + Photo.FileName;
                    Photo.SaveAs(path1 + "/" + NewPhoto);
                    string _photo = NewPhoto;
                    NewProduct.Photo = _photo;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    var result = client.PostAsync("/api/Product", NewProduct, new JsonMediaTypeFormatter()).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(NewProduct);
        }

        #endregion

        #region Edit Produt
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductDTO ProdInfo = new ProductDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<ProductDTO>(ProdResponse);
                }
            }

            if (ProdInfo == null)
            {
                return HttpNotFound();
            }
            return View(ProdInfo);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Photo,Price")] ProductDTO NewProduct, HttpPostedFileBase Photo)
        {
            ProductAppService appService = new ProductAppService();
            if (ModelState.IsValid)
            {
                //if (Photo != null)
                //{
                //    string path1 = Server.MapPath("~/Content/Image");
                //    if (!Directory.Exists(path1))
                //    {
                //        Directory.CreateDirectory(path1);
                //    }
                //    var NewPhoto = Guid.NewGuid().ToString() + Photo.FileName;
                //    Photo.SaveAs(path1 + "/" + NewPhoto);
                //    string _photo = NewPhoto;
                //    NewProduct.Photo = _photo;
                //}

                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(Baseurl);
                //    var result = client.PutAsync("/api/Product", NewProduct, new JsonMediaTypeFormatter()).Result;
                //    if (result.IsSuccessStatusCode)
                //    {
                //        return RedirectToAction("Index");
                //    }
                //}
                if(await appService.EditProduct(NewProduct, Photo))
                    return RedirectToAction("Index");
                ModelState.AddModelError("", "Somthing Went Wrong Please Try Again");
            }
            return View(NewProduct);
        }
        #endregion

        #region Delete Poduct

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ProductDTO ProdInfo = new ProductDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    ProdInfo = JsonConvert.DeserializeObject<ProductDTO>(ProdResponse);
                }
            }

            if (ProdInfo == null)
            {
                return HttpNotFound();
            }
            return View(ProdInfo);
        }

        [HttpPost]
        public async Task<ActionResult> Delete (int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.DeleteAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

    }
}
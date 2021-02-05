using Newtonsoft.Json;
using OfficeOpenXml;
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
using System.Web.UI;
using System.Web.UI.WebControls;
using Task.Business;
using Task.Helpers;
using Task.Models.DTO_Api;
using Task.Models.DTO_MVC;

namespace Task.Controllers.MVC
{
    public class ProductController : Controller
    {
        ProductAppService appService = new ProductAppService();

        public async Task<ActionResult> Index()
        {
            List<ProductDTO> ProdInfo = new List<ProductDTO>();
            ProdInfo = await appService.GetAllProducts();
            return View(ProdInfo);
        }

        public async Task<ActionResult> Details(int? id)
        {
            ProductDTO ProdInfo = await appService.ShowProductById(id.Value);

            if (ProdInfo == null)
                return HttpNotFound();
           
            return View(ProdInfo);
        }

        public async Task<ActionResult> search(string Name)
        {
            List<ProductDTO> listProduct = new List<ProductDTO>();
            listProduct = await appService.ShowProductsBySearch(Name);
            return View("Index", listProduct);
        }

        #region Create Product

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Name,Photo,Price")] ProductDTO NewProduct, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if(await appService.CreateProduct(NewProduct,Photo))
                    return RedirectToAction("Index");
            }
            return View(NewProduct);
        }

        #endregion

        #region Edit Produt
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            ProductDTO ProdInfo = await appService.ShowProductById(id.Value);

            if (ProdInfo == null)
                return HttpNotFound();

            return View(ProdInfo);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Photo,Price")] ProductDTO NewProduct, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if(await appService.EditProduct(NewProduct, Photo))
                    return RedirectToAction("Index");
            }
            return View(NewProduct);
        }
        #endregion

        #region Delete Poduct

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            ProductDTO ProdInfo = await appService.ShowProductById(id.Value);
            if (ProdInfo == null)
                return HttpNotFound();

            return View(ProdInfo);
        }

        [HttpPost]
        public async Task<ActionResult> Delete (int id)
        {
            await appService.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Export Excel Products

        public void DownloadExcel()
        {
            var products = new System.Data.DataTable("TableProduct");
            products.Columns.Add("Id", typeof(int));
            products.Columns.Add("Name", typeof(string));
            products.Columns.Add("Price", typeof(int));
            products.Columns.Add("LastUpdate", typeof(DateTime));

            ProductRepository Product_Repository = new ProductRepository();
            var collection = Product_Repository.GetAllProducts();

            foreach (var item in collection)
            {
                products.Rows.Add(item.Id,item.Name, item.Price , item.LastUpdate);
            }

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=product_catalog..xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        #endregion
    }
}
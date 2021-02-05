using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Task.Models;
using Task.Models.DTO_Api;

namespace Task.Controllers.Api
{
    public class ProductController : ApiController
    {
        ProductRepository Product_Repository = new ProductRepository();

        public IHttpActionResult GetAllProducts()
        {
            return Ok(Product_Repository.GetAllProducts());
        }

        public IHttpActionResult GetProductById(int id)
        {
            if(id != default)
            {
                var prodInfo = Product_Repository.GetProductsById(id);
                if (prodInfo != null)
                    return Ok(prodInfo);
                else
                    return NotFound();
            }
            return BadRequest();
        }

        public IHttpActionResult Post(Product NewProduct)
        {
            if (ModelState.IsValid)
            {
                Product_Repository.AddProduct(NewProduct);

                return Ok();
            }
            return BadRequest(ModelState);
        }

        public IHttpActionResult Put( Product product)
        {
            if (product != null)
            {
                var Prod = Product_Repository.UpdateProduct(product);
                if (Prod != null)
                    return Ok();
                else
                    return NotFound();
            }
            return BadRequest();
        }

        public IHttpActionResult Delete(int id)
        {
            if (id == default)
                return BadRequest();

            var Deleted = Product_Repository.DeleteProduct(id);
            if (Deleted == true)
                return Ok();
            else
                return NotFound();
        }

        public IHttpActionResult GetProductsFromSearch(string Name)
        {
            var listProduct = Product_Repository.SearchProduct(Name);
            if (listProduct == null)
                return NotFound();

            return Ok(listProduct);
        }
    }
}

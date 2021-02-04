using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task.Models.DTO_Api
{
    public class ProductRepository
    {
        Context _context;
        public ProductRepository()
        {
            _context = new Context();
        }

        public List<Product> GetAllProducts()
        {
            return _context.products.ToList();
        }

        public Product GetProductsById(int id)
        {
            return _context.products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product NewProduct)
        {
            var Date = DateTime.Now;
            NewProduct.LastUpdate = Date;
            _context.products.Add(NewProduct);
            _context.SaveChanges();
        }

        public Product UpdateProduct( Product NewProduct)
        {
            var ShowProduct = _context.products.FirstOrDefault(p => p.Id == NewProduct.Id);
            if (ShowProduct != null)
            {
                ShowProduct.LastUpdate = DateTime.Now;
                ShowProduct.Name = NewProduct.Name;
                ShowProduct.Price = NewProduct.Price;
                ShowProduct.Photo = NewProduct.Photo;
                _context.SaveChanges();
            
                return ShowProduct;
            }
            return null;
        }

        public bool DeleteProduct (int id)
        {
                var Prod = _context.products.FirstOrDefault(p => p.Id == id);
                if (Prod != null)
                {
                    _context.products.Remove(Prod);
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
        }

        protected void Dispose()
        {
            _context.Dispose();
        }
    }
}
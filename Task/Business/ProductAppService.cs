using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Task.Helpers;
using Task.Models.DTO_MVC;

namespace Task.Business
{
    public class ProductAppService
    {
        private FileUploaderHelper FileUploaderHelper { get; set; }
        private WebRequestHelper WebRequestHelper { get; set; }
        public ProductAppService()
        {
            FileUploaderHelper = new FileUploaderHelper();
            WebRequestHelper = new WebRequestHelper();
        }
        public async Task<bool> EditProduct(ProductDTO product, HttpPostedFileBase image)
        {
            bool result = default;
            if (product == null)
                return result;

            string productImagePath = null;
            if (image != null)
                productImagePath = FileUploaderHelper.UploadImage(image);
            if (string.IsNullOrWhiteSpace(productImagePath))
                product.Photo = productImagePath;
            if (await WebRequestHelper.SendPutRequest(product))
                result = true;

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Task.Helpers
{
    public class FileUploaderHelper
    {
        public string UploadImage(HttpPostedFileBase image)
        {
            string path1 = HttpContext.Current.Server.MapPath("~/Content/Image");
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            var NewPhoto = Guid.NewGuid().ToString() + image.FileName;
            image.SaveAs(path1 + "/" + NewPhoto);
            return NewPhoto;
        }
    }
}
using OAuth.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OAuth.Controllers
{
    [Authorize]
    public class ImageController : ApiController
    {
        [HttpPost]
        public IHttpActionResult CompressImage()
        {
            var fils = HttpContext.Current.Request.RequestContext.HttpContext.Request.Files;
            var fileImage = fils["FileImage[0]"];
            Image image = Image.FromStream(fileImage.InputStream);
            byte[] imageByte=  Compress.CompressImage2(image);
            //byte[] imageByte2 = Compress.SaveImageToByteArray(image);
            MemoryStream ms = new MemoryStream(imageByte);
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/Image/") + "" + Guid.NewGuid().ToString() + ".jpg";
            FileStream fs = new FileStream(path,FileMode.Create);
            ms.WriteTo(fs);
            ms.Close();
            fs.Close();
            return Json(imageByte);
        }
    }
}

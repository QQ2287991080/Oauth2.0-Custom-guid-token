using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace OAuth.Models
{
    public class Compress
    {

        public static byte[] SaveImageToByteArray(Image image, int jpegQuality = 90)
        {
            using (var ms = new MemoryStream())
            {
                var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)jpegQuality);
                image.Save(ms, jpegEncoder, encoderParameters);
                return ms.ToArray();
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public static byte[] CompressImage(Image img)
        {

            int originalwidth = img.Width;//图片宽度
            int originalheight = img.Height;//图片高度
            Bitmap bmpimage = new Bitmap(originalwidth, originalheight);//初始化System.Drawing的一个新实例。指定的位图类大小。
            Graphics gf = Graphics.FromImage(bmpimage);//创建一个新的System.Drawing。来自指定系统的图形。
            // 获取或设置此系统的呈现质量。
            gf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //获取或设置绘制到此系统的合成图像的呈现质量。
            gf.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //获取或设置与此系统关联的插值模式。
            gf.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //初始化System.Drawing的一个新实例。指定的矩形类地点和大小。
            Rectangle rect = new Rectangle(0, 0, originalwidth, originalheight);
            //绘制指定系统的指定部分。指定位置的图像位置和指定的大小。
            gf.DrawImage(img, rect, 0, 0, originalwidth, originalheight, GraphicsUnit.Pixel);
            byte[] imagearray;
            using (MemoryStream ms = new MemoryStream())
            {
                bmpimage.Save(ms, ImageFormat.Jpeg);
                imagearray = ms.ToArray();
            }
            return imagearray;
        }

        public static byte[] CompressImage2(Image img)
        {
            int sW = 0, sH = 0;
            int dHeight = img.Width/3, dWidth =img.Height/3 ;

            //按比例缩放  
            Size tem_size = new Size(img.Width, img.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)//如果图片宽度大于指定长度或宽度
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))//如果图片宽度*指定高度>图片宽度*指定宽度
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
           
            Bitmap bmpimage = new Bitmap(dWidth, dWidth);//初始化System.Drawing的一个新实例。指定的位图类大小。
            Graphics gf = Graphics.FromImage(bmpimage);//创建一个新的System.Drawing。来自指定系统的图形。
            gf.Clear(Color.White);
            // 获取或设置此系统的呈现质量。
            gf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //获取或设置绘制到此系统的合成图像的呈现质量。
            gf.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
            //获取或设置与此系统关联的插值模式。
            gf.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //初始化System.Drawing的一个新实例。指定的矩形类地点和大小。
            Rectangle rect = new Rectangle((dWidth - sW) / 2, (dWidth - sH) / 2, sW, sH);
            //绘制指定系统的指定部分。指定位置的图像位置和指定的大小。
            gf.DrawImage(img, rect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            gf.Dispose();
            byte[] imagearray;
            using (MemoryStream ms = new MemoryStream())
            {
                bmpimage.Save(ms, ImageFormat.Jpeg);
                imagearray = ms.ToArray();
            }
            bmpimage.Dispose();
            return imagearray;
        }
    }
}
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using PrintControl.Common;

namespace PrintControl.Utils
{
    public static class Pdf2JpgUtils
    {
        #region 文件转换
        public static void Pdf2Jpg(string sourcePath, string targetPath, int? pages, string folderName)
        {
            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentNullException("sourcePath");
            if (string.IsNullOrEmpty(targetPath))
                throw new ArgumentNullException("targetPath");
            if (!System.IO.File.Exists(sourcePath))
                throw new Exception("文件不存在：" + sourcePath);
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(sourcePath))
            {
                Pdf2Jpg(pdfDocument, targetPath, pages, folderName);
            }
        }

        public static void Pdf2Jpg(Stream sourceStream, string targetPath, int? pages)
        {
            if (sourceStream == null)
                throw new ArgumentNullException("sourceStream");
            using (var pdfDocument = PdfiumViewer.PdfDocument.Load(sourceStream))
            {
                Pdf2Jpg(pdfDocument, targetPath, pages);
            }
        }

        public static void Pdf2Jpg(PdfiumViewer.PdfDocument pdfDocument, string targetPath, int? pages)
        { 
            Pdf2Jpg(pdfDocument, targetPath, pages, "");
        }
        public static void Pdf2Jpg(PdfiumViewer.PdfDocument pdfDocument, string targetPath, int? pages, string folderName)
        {
            if (string.IsNullOrEmpty(targetPath))
                throw new ArgumentNullException("targetPath");

            var dir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (pdfDocument.PageCount == 1)
                pages = 1;

            var imageDir = Path.Combine(dir, string.IsNullOrEmpty(folderName) ? Guid.NewGuid().ToString() : folderName);
            Directory.CreateDirectory(imageDir);
            if (pages != null)
            {
                pages--;//PdfiumViewer.PdfDocument的页数是从0开始的
                using (var image = pdfDocument.Render(pages.Value, ConfigDefine.Pdf2JpgResolution, ConfigDefine.Pdf2JpgResolution, PdfiumViewer.PdfRenderFlags.CorrectFromDpi))
                {
                    if (ConfigDefine.Pdf2JpgScaleRatio != 0)
                    {
                        Stream imageStream = new MemoryStream();
                        image.Save(imageStream, ImageFormat.Jpeg);
                        int imgWidth = Convert.ToInt32(image.Width * ConfigDefine.Pdf2JpgScaleRatio);
                        int imgHeight = Convert.ToInt32(image.Height * ConfigDefine.Pdf2JpgScaleRatio);
                        using (var imgScaleStream = ImageScale2Stream(imageStream, imgWidth, imgHeight, ""))
                        {
                            using (var imgScale = System.Drawing.Image.FromStream(imgScaleStream))
                            {
                                imgScale.Save(Path.Combine(imageDir, folderName + ".jpg"));
                                //imgScale.Save(targetPath);
                            }
                        }
                    }
                    else
                    {
                        image.Save(Path.Combine(imageDir, folderName + ".jpg"));
                        //image.Save(targetPath);
                    }
                }
            }
            else
            {
                for (pages = 0; pages < pdfDocument.PageCount; pages++)
                {
                    using (var image = pdfDocument.Render(pages.Value, ConfigDefine.Pdf2JpgResolution, ConfigDefine.Pdf2JpgResolution, PdfiumViewer.PdfRenderFlags.CorrectFromDpi))
                    {
                        if (ConfigDefine.Pdf2JpgScaleRatio != 0)
                        {
                            Stream imageStream = new MemoryStream();
                            image.Save(imageStream, ImageFormat.Jpeg);
                            int imgWidth = Convert.ToInt32(image.Width * ConfigDefine.Pdf2JpgScaleRatio);
                            int imgHeight = Convert.ToInt32(image.Height * ConfigDefine.Pdf2JpgScaleRatio);
                            using (var imgScaleStream = ImageScale2Stream(imageStream, imgWidth, imgHeight, ""))
                            {
                                using (var imgScale = System.Drawing.Image.FromStream(imgScaleStream))
                                {
                                    imgScale.Save(Path.Combine(imageDir, pages + ".jpg"));
                                }
                            }
                        }
                        else
                        {
                            image.Save(Path.Combine(imageDir, pages + ".jpg"));
                        }

                    }
                }
                //图片拼接
                //MergerImage(imageDir, targetPath);
                /*
                try
                {
                    //删除文件夹
                    Directory.Delete(imageDir, true);
                }
                catch (Exception ex)
                {
                    //Send(string.Format("删除临时目录出错：{0};其他错误信息：{1}", imageDir, ex));
                    throw new ArgumentNullException(string.Format("删除临时目录出错：{0};其他错误信息：{1}", imageDir, ex));
                }
                 * */
            }
        }

        public static void Pdf2Jpg(Aspose.Pdf.Document pdfDocument, string targetPath, int? pages)
        {
            if (string.IsNullOrEmpty(targetPath))
                throw new ArgumentNullException("targetPath");

            var dir = Path.GetDirectoryName(targetPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (pdfDocument.Pages.Count == 1)
                pages = 1;

            if (pages != null)
            {
                using (var imageStream = new FileStream(targetPath, FileMode.Create))
                {
                    var resolution = new Aspose.Pdf.Devices.Resolution(ConfigDefine.Pdf2JpgResolution);
                    var jpegDevice = new Aspose.Pdf.Devices.JpegDevice(resolution, 100);
                    jpegDevice.Process(pdfDocument.Pages[pages.Value], imageStream);
                    imageStream.Close();
                }
            }
            else
            {
                var imageDir = Path.Combine(dir, Guid.NewGuid().ToString());
                Directory.CreateDirectory(imageDir);
                for (pages = 1; pages <= pdfDocument.Pages.Count; pages++)
                {
                    using (var imageStream = new FileStream(Path.Combine(imageDir, pages + ".jpg"), FileMode.Create))
                    {
                        var resolution = new Aspose.Pdf.Devices.Resolution(ConfigDefine.Pdf2JpgResolution);
                        var jpegDevice = new Aspose.Pdf.Devices.JpegDevice(resolution, 100);
                        jpegDevice.Process(pdfDocument.Pages[pages.Value], imageStream);
                        imageStream.Close();
                    }
                }
                MergerImage(imageDir, targetPath);
                try
                {
                    Directory.Delete(imageDir, true);
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException(string.Format("删除临时目录出错：{0};其他错误信息：{1}", imageDir, ex));
                }
            }
        }

        public static void MergerImage(string imageDir, string targetPath)
        {
            //var imageList = Directory.GetFiles(imageDir).OrderBy(f => f).Select(file => new Bitmap(file)).ToList();

            IList<Image> imageList = new List<Image>();
            //var imgs = Directory.GetFiles(imageDir);
            foreach (var file in Directory.GetFiles(imageDir))
            {
                //Image image = new Bitmap(file);
                imageList.Add(new Bitmap(file));
            }

            var x = 0;
            var y = 0;

            foreach (var t in imageList)
            {
                x = x > t.Width ? x : t.Width;
                y += t.Height + 20;
            }

            using (var newImg = new Bitmap(x, y))
            {
                x = 0;
                y = 0;

                using (var g = Graphics.FromImage(newImg))
                {
                    g.Clear(Color.White);

                    foreach (var t in imageList)
                    {
                        g.DrawImage(t, x, y, t.Width, t.Height);
                        y += t.Height + 20;
                        t.Dispose();
                    }
                }

                newImg.Save(targetPath, ImageFormat.Jpeg);
            }
        }
        public static Image MergerImage(string imageDir)
        {
            //var imageList = Directory.GetFiles(imageDir).OrderBy(f => f).Select(file => new Bitmap(file)).ToList();

            IList<Image> imageList = new List<Image>();
            //var imgs = Directory.GetFiles(imageDir);
            foreach (var file in Directory.GetFiles(imageDir))
            {
                //Image image = new Bitmap(file);
                imageList.Add(new Bitmap(file));
            }

            var x = 0;
            var y = 0;

            foreach (var t in imageList)
            {
                x = x > t.Width ? x : t.Width;
                y += t.Height + 20;
            }

            using (var newImg = new Bitmap(x, y))
            {
                x = 0;
                y = 0;

                using (var g = Graphics.FromImage(newImg))
                {
                    g.Clear(Color.White);

                    foreach (var t in imageList)
                    {
                        g.DrawImage(t, x, y, t.Width, t.Height);
                        y += t.Height + 20;
                        t.Dispose();
                    }
                }

                return newImg;
            }
        }

        public static void Image2Pdf(string imageFile, string pdfFile, bool reportPageRotate, string mimeType, int? imgWidth, int? imgHeight)
        {
            Image2Pdf(imageFile, pdfFile, "A4", false, "", null, null);
        }
        public static void Image2Pdf(string imageFile, string pdfFile, string reportPageSize, bool reportPageRotate, string mimeType, int? imgWidth, int? imgHeight)
        {
            if (string.IsNullOrEmpty(imageFile))
                throw new ArgumentNullException("imageFile");
            if (!System.IO.File.Exists(imageFile))
                throw new Exception("文件不存在：" + imageFile);

            var dir = Path.GetDirectoryName(pdfFile);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            iTextSharp.text.Document document = null;
            iTextSharp.text.pdf.PdfWriter writer = null;
            try
            {
                iTextSharp.text.Rectangle pageSize = iTextSharp.text.PageSize.A4;
                switch (reportPageSize)
                {
                    case "A4":
                        pageSize = iTextSharp.text.PageSize.A4;
                        break;
                    case "A5":
                        pageSize = iTextSharp.text.PageSize.A5;
                        break;
                    case "B5":
                        pageSize = iTextSharp.text.PageSize.B5;
                        break;
                    default:
                        pageSize = iTextSharp.text.PageSize.A4;
                        break;
                }
                var rectPageSize = new iTextSharp.text.Rectangle(pageSize);
                if (reportPageRotate == true)
                {
                    rectPageSize = rectPageSize.Rotate();
                }
                document = new iTextSharp.text.Document(rectPageSize, 25, 25, 25, 25);
                writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Create));
                document.Open();

                Image sourceImg = null;
                if (imgWidth != null || imgHeight != null)
                {
                    var fileStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read);
                    var ms = ImageScale2Stream(fileStream, imgWidth, imgHeight, mimeType);
                    sourceImg = System.Drawing.Image.FromStream(ms);
                }
                else
                {
                    sourceImg = System.Drawing.Image.FromFile(imageFile);
                }

                var pdfImage = iTextSharp.text.Image.GetInstance(sourceImg, System.Drawing.Imaging.ImageFormat.Png);
                pdfImage.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                pdfImage.ScaleToFit(document.Right, document.Top - document.TopMargin);

                document.Add(pdfImage);
                sourceImg.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (document != null)
                    document.Close();
                if (writer != null)
                    writer.Close();
            }
        }

        public static MemoryStream ImageScale2Stream(Stream imageStream, int? scaleWidth, int? scaleHeight, string mimeType)
        {
            try
            {
                using (var original = new Bitmap(imageStream))
                {
                    if (scaleWidth == null && scaleHeight != null)
                    {
                        scaleWidth = Convert.ToInt32(1.0 * original.Width / original.Height * scaleHeight.Value);
                    }
                    else if (scaleWidth != null && scaleHeight == null)
                    {
                        scaleHeight = Convert.ToInt32(1.0 * scaleWidth.Value * original.Height / original.Width);
                    }
                    else
                    {
                        //根据宽高的比例，按宽或者高比例小的值设置缩放图片的宽和高
                        var w = Convert.ToInt32(1.0 * original.Width / original.Height * scaleHeight.Value);
                        if (scaleWidth >= w)
                        {
                            scaleWidth = w;
                        }
                        else
                        {
                            scaleHeight = Convert.ToInt32(1.0 * scaleWidth.Value * original.Height / original.Width);
                        }
                    }

                    ImageFormat imageFormat = null;
                    switch (mimeType)
                    {
                        case "image/bmp":
                            imageFormat = ImageFormat.Bmp;
                            break;
                        case "image/png":
                            imageFormat = ImageFormat.Png;
                            break;
                        case "image/gif":
                            imageFormat = ImageFormat.Gif;
                            break;
                        default:
                            imageFormat = ImageFormat.Jpeg;
                            break;
                    }

                    if (scaleWidth > original.Width || scaleHeight > original.Height)
                    {
                        var fs = new System.IO.MemoryStream();
                        original.Save(fs, imageFormat);
                        return fs;
                    }
                    else
                    {
                        using (var image = new Bitmap(scaleWidth.Value, scaleHeight.Value))
                        {
                            var graph = Graphics.FromImage(image);
                            graph.CompositingQuality = CompositingQuality.HighQuality;
                            graph.SmoothingMode = SmoothingMode.HighQuality;
                            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graph.DrawImage(original, new Rectangle(0, 0, scaleWidth.Value, scaleHeight.Value),
                                new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                            graph.Save();
                            graph.Dispose();

                            var fs = new System.IO.MemoryStream();
                            image.Save(fs, imageFormat);
                            return fs;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("缩放图片时出错：{0}", ex));
            }

            return null;
        }

        public static byte[] ImageScale(Stream imageStream, int? scaleWidth, int? scaleHeight, string mimeType)
        {
            try
            {
                return ImageScale2Stream(imageStream, scaleWidth, scaleHeight, mimeType).ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("缩放图片时出错：{0}", ex));
            }

            return null;
        }

        public static byte[] ImageScale(string imagePath, int? scaleWidth, int? scaleHeight, string mimeType)
        {
            try
            {
                var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

                return ImageScale(fileStream, scaleWidth, scaleHeight, mimeType);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException(string.Format("缩放图片时出错：{0};其他错误信息：{1}", imagePath, ex));
            }

            return null;
        }

        #endregion

    }
}

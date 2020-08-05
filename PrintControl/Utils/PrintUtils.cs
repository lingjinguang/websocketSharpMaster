using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using PrintControl.Model;
using PrintControl.Common;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PrintControl.Utils
{
    public static class PrintUtils
    {
        private static Font printFont;
        private static StreamReader streamToPrint;
        static PrintUtils()
        {
        }
        /// <summary>
        /// 打印图片（单张）
        /// </summary>
        public static string PrintImage(string data, string printType, string direction, string printerName, string paperName)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (string.IsNullOrEmpty(data))
                    return "要打印的数据不存在";

                Image image = printType == "FILEPATH" ? new Bitmap(data) : Base64ToImg(data);

                PrintDocument pd = new PrintDocument();
                PrinterSettings.PaperSizeCollection paperSizes = pd.PrinterSettings.PaperSizes; //获取打印纸张
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                //PaperSize ps = new PaperSize("Custom Size 1", 827, 1169);
                pd.DefaultPageSettings.PaperSize = getPaperSize(paperSizes, paperName) ?? pd.DefaultPageSettings.PaperSize;
                pd.PrintPage += (o, e) =>
                {
                    PageSettings settings = e.PageSettings;
                    var paperHeight = settings.PaperSize.Height;
                    var paperWidth = settings.PaperSize.Width;
                    //Rectangle m = e.MarginBounds;
                    int margin = 10, imgHeight = 0, imgWidth = 0;
                    ImageScaling(image, paperHeight, paperWidth, margin, direction, ref imgHeight, ref imgWidth);
                    if (direction == "2")   //2:横向打印（html转pdf的有问题，html转pdf不考虑宽度，转出来标准的pdf页，分页也已经完成，在这部分就处理不了）
                    {
                        e.Graphics.TranslateTransform(0, paperHeight);  //旋转原点
                        e.Graphics.RotateTransform(-90.0F); //旋转角度
                    }
                    e.Graphics.DrawImage(image, margin, margin, imgWidth, imgHeight);

                    //e.Graphics.DrawImage(image, m);
                    /* 小于等于4961*7016的正常
                    Point loc = new Point(10, 10);
                    e.Graphics.DrawImage(image, loc);
                    */
                };
                pd.Print();
                image.Dispose();

                return "打印成功！";
            }
            catch (Exception ex)
            {
                return "打印出错【" + ex.Message + "】";
            }
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        public static string PreView(string filePath, string eventType, string direction, string printerName, string paperName)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (!Directory.Exists(filePath) && !File.Exists(filePath))
                    return "要打印的数据不存在";

                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                pd.DefaultPageSettings.PaperSize = getPaperSize(pd.PrinterSettings.PaperSizes, paperName) ?? pd.DefaultPageSettings.PaperSize;
                var fileListIndex = 0;
                //var fileList = Directory.GetFiles(filePath);

                List<string> fileList = new List<string>();
                if (File.Exists(filePath)) //路径是jpg图片
                {
                    fileList.Add(filePath);
                }
                else
                {
                    fileList = new List<string>(Directory.GetFiles(filePath));
                }
                if (fileList.Count == 0)
                    return "要打印的数据不存在";
                pd.PrintPage += (o, e) =>
                {
                    PageSettings settings = e.PageSettings;
                    var paperHeight = settings.PaperSize.Height;
                    var paperWidth = settings.PaperSize.Width;
                    int margin = 10, imgHeight = 0, imgWidth = 0;

                    using (Image image = new Bitmap(fileList[fileListIndex]))
                    {
                        ImageScaling(image, paperHeight, paperWidth, margin, direction, ref imgHeight, ref imgWidth);
                        if (direction == "2")   //2:横向打印（html转pdf的有问题，html转pdf不考虑宽度，转出来标准的pdf页，分页也已经完成，在这部分就处理不了）
                        {
                            e.Graphics.TranslateTransform(0, paperHeight);  //旋转原点
                            e.Graphics.RotateTransform(-90.0F); //旋转角度
                        }

                        e.Graphics.DrawImage(image, margin, margin, imgWidth, imgHeight);
                    }
                    if (fileListIndex < fileList.Count - 1)
                    {
                        e.HasMorePages = true;  //HaeMorePages属性为True时，PrintPage的回调函数就会被再次调用，打印一个页面。 
                        fileListIndex++;
                    }
                    else
                    {
                        //预览界面点击打印需要把索引重新初始化
                        fileListIndex = 0;
                    }
                };

                //打印结束-删除一些创建的文件
                //pd.EndPrint += new PrintEventHandler();

                if (eventType == "PRINT")
                {
                    pd.Print();
                }
                else if (eventType == "PREVIEW")
                {
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.WindowState = FormWindowState.Maximized;
                    printPreviewDialog.Document = pd;
                    printPreviewDialog.ShowDialog();
                }
                deleteTemFile();
                return "打印成功！";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// 打印预览（Base64）
        /// </summary>
        public static string PreViewByBase64(string data, string eventType, string direction, string printerName, string paperName)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (string.IsNullOrEmpty(data))
                    return "要打印的数据不存在";

                Image image = Base64ToImg(data);

                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                pd.DefaultPageSettings.PaperSize = getPaperSize(pd.PrinterSettings.PaperSizes, paperName) ?? pd.DefaultPageSettings.PaperSize;
                pd.PrintPage += (o, e) =>
                {
                    PageSettings settings = e.PageSettings;
                    var paperHeight = settings.PaperSize.Height;
                    var paperWidth = settings.PaperSize.Width;
                    int margin = 10;
                    var imgHeight = 0;
                    var imgWidth = 0;
                    ImageScaling(image, paperHeight, paperWidth, margin, direction, ref imgHeight, ref imgWidth);
                    if (direction == "2")   //2:横向打印（html转pdf的有问题，html转pdf不考虑宽度，转出来标准的pdf页，分页也已经完成，在这部分就处理不了）
                    {
                        e.Graphics.TranslateTransform(0, paperHeight);  //旋转原点
                        e.Graphics.RotateTransform(-90.0F); //旋转角度
                    }
                    e.Graphics.DrawImage(image, margin, margin, imgWidth, imgHeight);
                };
                if (eventType == "PRINT")
                {
                    pd.Print();
                }
                else if (eventType == "PREVIEW")
                {
                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.WindowState = FormWindowState.Maximized;
                    //printPreviewDialog.PrintPreviewControl.
                    //printPreviewDialog.PrintPreviewControl.StartPage = 0;
                    printPreviewDialog.Document = pd;
                    printPreviewDialog.ShowDialog();

                }
                return "打印成功！";
            }
            catch (Exception ex)
            {
                return "打印出错【" + ex.Message + "】";
            }
        }
        public static void ImageScaling(Image image, int paperHeight, int paperWidth, int margin, string direction, ref int imgHeight, ref int imgWidth)
        {
            if (direction == "2")   //2:横向打印
            {
                if ((double)image.Height / (double)image.Width > (double)paperWidth / (double)paperHeight) // 图片等比例放大，高度超出了，先确定高度
                {
                    imgHeight = (int)(paperWidth - margin * 2);
                    imgWidth = (int)((double)image.Width / (double)image.Height * imgHeight - margin * 2);
                }
                else
                {
                    imgWidth = (int)(paperHeight - margin * 2);
                    imgHeight = (int)((double)image.Height / (double)image.Width * imgWidth - margin * 2);
                }
            }
            else
            {
                if ((double)image.Width / (double)image.Height > (double)paperWidth / (double)paperHeight) // 图片等比例放大，宽度超出了，先确定宽度
                {
                    imgWidth = (int)(paperWidth - margin * 2);
                    imgHeight = (int)((double)image.Height * (double)imgWidth / (double)image.Width - margin * 2);
                }
                else
                {
                    imgHeight = (int)(paperHeight - margin * 2);
                    imgWidth = (int)((double)image.Width / (double)image.Height * imgHeight - margin * 2);
                }
            }
        }
        #region 打印PDF (xp虚拟机测试出错，win10可使用)
        /// <summary>
        /// 打印PDF
        /// </summary>
        public static string PrintPdf(string filePath, string printType)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (!Directory.Exists(filePath) && !File.Exists(filePath))
                    return "要打印的数据不存在";

                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(filePath))
                {
                    for (int? pages = 0; pages < pdfDocument.PageCount; pages++)
                    {
                        using (var image = pdfDocument.Render(pages.Value, ConfigDefine.Pdf2JpgResolution, ConfigDefine.Pdf2JpgResolution, PdfiumViewer.PdfRenderFlags.CorrectFromDpi))
                        {
                            PrintDocument pd = new PrintDocument();
                            pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                            pd.PrintPage += (o, e) =>
                            {
                                PrintPage(o, e, image, false);
                            };
                            pd.Print();
                        }
                    }
                }
                return "打印成功！";
            }
            catch (Exception ex)
            {
                return "打印出错【" + ex.Message + "】";
            }
        }
        public static void PrintPage(object sender, PrintPageEventArgs e, Image image, Boolean hasMorePages)
        {
            PageSettings settings = e.PageSettings;
            var paperHeight = settings.PaperSize.Height;
            var paperWidth = settings.PaperSize.Width;
            //Rectangle m = e.MarginBounds;
            int margin = 10;
            var imgHeight = 0;
            var imgWidth = 0;
            if ((double)image.Width / (double)image.Height > (double)paperWidth / (double)paperHeight) // 图片等比例放大，宽度超出了，先确定宽度
            {
                imgWidth = (int)(paperWidth - margin * 2);
                imgHeight = (int)((double)image.Height * (double)imgWidth / (double)image.Width - margin * 2);
            }
            else
            {
                imgHeight = (int)(paperHeight - margin * 2);
                imgWidth = (int)((double)image.Width / (double)image.Height * imgHeight - margin * 2);
            }
            e.Graphics.DrawImage(image, margin, margin, imgWidth, imgHeight);

            if (hasMorePages)
            {
                e.HasMorePages = true;  //HaeMorePages属性为True时，PrintPage的回调函数就会被再次调用，打印一个页面。 
            }
            //e.Graphics.DrawImage(image, m);
            /* 小于等于4961*7016的正常
            Point loc = new Point(10, 10);
            e.Graphics.DrawImage(image, loc);
            */
        }
        #endregion
        #region 打印预览PDF (xp虚拟机测试出错，win10可使用)
        /// <summary>
        /// 打印预览PDF
        /// </summary>
        public static string PreViewPdf(string filePath, string printType)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (!Directory.Exists(filePath) && !File.Exists(filePath))
                    return "要打印的数据不存在";

                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(filePath))
                {
                    int? pages = 0;
                    PrintDocument pd = new PrintDocument();
                    pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                    pd.PrintPage += (o, e) =>
                    {
                        PreViewPage(o, e, pdfDocument, ref pages);
                    };

                    PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.WindowState = FormWindowState.Maximized;
                    //printPreviewDialog.PrintPreviewControl.
                    //printPreviewDialog.PrintPreviewControl.StartPage = 0;
                    printPreviewDialog.Document = pd;
                    printPreviewDialog.ShowDialog();
                    //pd.Print();
                }
                return "打印成功！";
            }
            catch (Exception ex)
            {
                return "打印出错【" + ex.Message + "】";
            }
        }
        public static void PreViewPage(object sender, PrintPageEventArgs e, PdfiumViewer.PdfDocument pdfDocument, ref int? pages)
        {
            PageSettings settings = e.PageSettings;
            var paperHeight = settings.PaperSize.Height;
            var paperWidth = settings.PaperSize.Width;
            int margin = 10;
            var imgHeight = 0;
            var imgWidth = 0;

            //int? pages = 0;
            using (var image = pdfDocument.Render(pages.Value, ConfigDefine.Pdf2JpgResolution, ConfigDefine.Pdf2JpgResolution, PdfiumViewer.PdfRenderFlags.CorrectFromDpi))
            {
                if ((double)image.Width / (double)image.Height > (double)paperWidth / (double)paperHeight) // 图片等比例放大，宽度超出了，先确定宽度
                {
                    imgWidth = (int)(paperWidth - margin * 2);
                    imgHeight = (int)((double)image.Height * (double)imgWidth / (double)image.Width - margin * 2);
                }
                else
                {
                    imgHeight = (int)(paperHeight - margin * 2);
                    imgWidth = (int)((double)image.Width / (double)image.Height * imgHeight - margin * 2);
                }
                e.Graphics.DrawImage(image, margin, margin, imgWidth, imgHeight);
            }
            if (pages < pdfDocument.PageCount - 1)
            {
                e.HasMorePages = true;  //HaeMorePages属性为True时，PrintPage的回调函数就会被再次调用，打印一个页面。 
                pages++;
            }
            else
            {
                //预览界面点击打印需要把索引重新初始化
                pages = 0;
            }
        }
        #endregion
        #region 打印Txt文件
        public static BaseResult PrintTxt(string data, string printType)
        {
            try
            {
                Setting setting = FileUtils.GetSetting();
                if (string.IsNullOrEmpty(setting.Name))
                    return BaseResult.Error("请先设置默认打印机");
                if (string.IsNullOrEmpty(data))
                    return BaseResult.Error("要打印的数据不存在");

                streamToPrint = printType == "FILEPATH" ? new StreamReader(data) : new StreamReader(data);

                try
                {
                    printFont = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (PdPrintPage);
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception e)
            {
                return BaseResult.Error(e.Message);
            }
            return BaseResult.Success("");
        }

        private static void PdPrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        #endregion
        #region 通用工具
        /// <summary>
        /// Base64转成Imgage数据类型
        /// </summary>
        private static Image Base64ToImg(string base64str)
        {
            byte[] arr = Convert.FromBase64String(base64str);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);
            return bmp;
        }
        /// <summary>
        /// 删除临时文件
        /// </summary>
        public static void deleteTemFile()
        {
            string[] files = new string[] { Path.Combine(Directory.GetCurrentDirectory(), "tempImg"), Path.Combine(Directory.GetCurrentDirectory(), "tempPdf") };
            foreach (var file in files)
            {
                foreach (string f in Directory.GetFileSystemEntries(file))
                {
                    if (string.IsNullOrEmpty(f)) return;
                    if (File.Exists(f))
                    {
                        //删除子文件
                        File.Delete(f);
                    }
                    else
                    {
                        //删除子文件夹
                        Directory.Delete(f, true);
                    }
                }
            }
        }
        #endregion
        #region 获取打印机、打印页信息、重新设置打印机
        public static string GetPrinterNames()
        {
            string printerNames = "";

            foreach (var name in PrinterSettings.InstalledPrinters)
            {
                printerNames += name + ",";
            }
            return string.IsNullOrEmpty(printerNames) ? printerNames : printerNames.Substring(0, printerNames.Length - 1);
        }
        public static string GetPageSizesByName(string printerName)
        {
            if (string.IsNullOrEmpty(printerName)) return "打印机名称不能为空！";
            //printerName 前端的打印配置有单独保存在数据库，所以在改变打印机时就重新保存打印机
            SaveSetting(printerName);

            PrintDocument pd = new PrintDocument();
            var paperSizes = pd.PrinterSettings.PaperSizes; //获取打印纸张
            string paper = "";
            foreach (PaperSize paperSize in pd.PrinterSettings.PaperSizes)
            {
                paper += Convert.ToString(paperSize.PaperName) + ",";
            }
            return string.IsNullOrEmpty(paper) ? paper : paper.Substring(0, paper.Length - 1);
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);
        /// <summary>
        /// 根据请求设置，重新设置默认打印机
        /// </summary>
        public static void SaveSetting(string name)
        {
            Setting setting = FileUtils.GetSetting();
            setting.Name = name;
            setting.IsDefault = true;
            Boolean result = FileUtils.SaveSetting(setting);
            if (result) SetDefaultPrinter(setting.Name);
            /*
            if (result2) //设置默认打印机
            {
                MainForm mainforom = new MainForm();
                mainforom.lab_defualtPrinterName.Text = setting.Name ?? "无";
                var d = mainforom.comboBox1.Items.IndexOf(name);
                mainforom.comboBox1.SelectedIndex = mainforom.comboBox1.Items.IndexOf(name);
            }
            else
            {
                MessageBox.Show("设置为默认打印机失败！");
            }
             * */
        }
        public static PaperSize getPaperSize(PrinterSettings.PaperSizeCollection paperSizes, string paperName)
        {
            //PaperSize ps = null;    //new PaperSize("Custom Size 1", 827, 1169)
            if (string.IsNullOrEmpty(paperName)) return null;
            foreach (PaperSize paperSize in paperSizes)
            {
                if (Convert.ToString(paperSize.PaperName) == paperName) 
                {
                    return paperSize;
                }
            }
            return null;
        }
        public static void getPaperSize(string paperName,ref int width, ref int height)
        {
            PrintDocument pd = new PrintDocument();
            foreach (PaperSize paperSize in pd.PrinterSettings.PaperSizes)
            {
                if (Convert.ToString(paperSize.PaperName) == paperName)
                {
                    width = paperSize.Width;
                    height = paperSize.Height;
                    break;
                }
            }
        }
        #endregion
    }
}

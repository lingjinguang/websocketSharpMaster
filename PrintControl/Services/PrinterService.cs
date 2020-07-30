using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using PrintControl.Model;
using PrintControl.Utils;

using System.Windows.Forms;

namespace PrintControl.Services
{
    public class PrinterService : WebSocketBehavior
    {
        private Font printFont;
        private StreamReader streamToPrint;

        private static PrinterService _instance { get; set; }
        /// <summary>
        /// 获得实例
        /// </summary>
        public static PrinterService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PrinterService();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
       
        public PrinterService()
        {

        }
        /// <summary>
        /// 打印图片
        /// </summary>
        public BaseResult PrintImage(string data, string print_type)
        {
            try
            {
                var setting = FileUtils.GetSetting();

                if (string.IsNullOrEmpty(setting.Name))
                    return BaseResult.Error("请先设置默认打印机");
                if(string.IsNullOrEmpty(data))
                    return BaseResult.Error("要打印的数据不存在");

                Image image = print_type == "FILEPATH" ? new Bitmap(data) : Base64ToImg(data);

                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;

                pd.PrintPage += (o, e) =>
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

                    //e.Graphics.DrawImage(image, m);
                    /* 小于等于4961*7016的正常
                    Point loc = new Point(10, 10);
                    e.Graphics.DrawImage(image, loc);
                    */
                };
                pd.Print();
                image.Dispose();

                return BaseResult.Success("");
            }
            catch (Exception e)
            {
                return BaseResult.Error(e.Message);
            }
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        public string PreView(string filePath, string eventType)
        {
            try
            {
                var setting = FileUtils.GetSetting();

                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (!Directory.Exists(filePath) && !File.Exists(filePath))
                    return "要打印的数据不存在";

                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
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
                    int margin = 10;
                    var imgHeight = 0;
                    var imgWidth = 0;

                    using (Image image = new Bitmap(fileList[fileListIndex])) 
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
                    //Rectangle m = e.MarginBounds;
                };

                //打印结束-删除一些创建的文件
                //pd.EndPrint += new PrintEventHandler(deleteFile(files));

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
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public void deleteFile(List<string> files)
        {
            foreach (var file in files)
            {
                Directory.Delete(file, true);
            }
        }
        /// <summary>
        /// 打印预览
        /// </summary>
        public string PreViewByBase64(string data, string eventType)
        {
            try
            {
                var setting = FileUtils.GetSetting();

                if (string.IsNullOrEmpty(setting.Name))
                    return "请先设置默认打印机";
                if (string.IsNullOrEmpty(data))
                    return "要打印的数据不存在";

                Image image = Base64ToImg(data);

                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = setting.Name;
                pd.PrintPage += (o, e) =>
                {
                    PageSettings settings = e.PageSettings;
                    var paperHeight = settings.PaperSize.Height;
                    var paperWidth = settings.PaperSize.Width;
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
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// Base64转成Imgage数据类型
        /// </summary>
        private Image Base64ToImg(string base64str)
        {
            byte[] arr = Convert.FromBase64String(base64str);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);
            return bmp;
        }
        #region 打印Txt文件
        private BaseResult PrintTxt(string data, string print_type)
        {
            try
            {
                var setting = FileUtils.GetSetting();

                if (string.IsNullOrEmpty(setting.Name))
                    return BaseResult.Error("请先设置默认打印机");
                if (string.IsNullOrEmpty(data))
                    return BaseResult.Error("要打印的数据不存在");

                streamToPrint = print_type == "FILEPATH" ? new StreamReader(data) : new StreamReader(data);
                
                try
                {
                    printFont = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (this.PdPrintPage);
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

        private void PdPrintPage(object sender, PrintPageEventArgs ev)
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
        protected override void OnClose(CloseEventArgs e)
        {
            //Sessions.Broadcast(String.Format("{0} got logged off...", _name));
        }

        /// <summary>
        /// 客户端发送send请求会被触发，接收请求数据
        /// </summary>
        protected override void OnMessage(MessageEventArgs e)
        {
            String result = "打印成功！";
            try
            {
                var data = JsonHelper.DeserializeJsonToObject<MessageEvent<String>>(e.Data);
                string eventType = Convert.ToString(data.event_type).ToUpper();
                string printType = Convert.ToString(data.print_type).ToUpper();
                switch (Convert.ToString(data.file_type).ToUpper())
                {
                    case "JPG":
                        if (eventType == "PRINT")
                        {
                            this.PrintImage(Convert.ToString(data.data), printType);
                            //result = this.PreView(Convert.ToString(data.data), eventType);
                        }
                        else if (eventType == "PREVIEW")
                        {
                            /*
                            if (printType == "FILEPATH")
                            {
                                result = this.PreView(Convert.ToString(data.data), eventType);
                            }
                            else 
                            {
                                result = this.PreViewByBase64(Convert.ToString(data.data), eventType);
                            }
                             * */
                            result = printType == "FILEPATH" ? this.PreView(Convert.ToString(data.data), eventType) : this.PreViewByBase64(Convert.ToString(data.data), eventType);
                        }
                        break;
                    case "TXT":
                        PrintTxt(Convert.ToString(data.data), printType);
                        break;
                    case "PDF":                     
                        string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "tempImg");
                        string fileName = Path.GetFileNameWithoutExtension(data.data); //通过完整路径取得pdf文件名称作为jpg的文件名
                        string targetPath = Path.Combine(tempDir, fileName + ".jpg"); 
                        string jpgFile = Path.Combine(tempDir, fileName + ".jpg");
                        string pdfFile = Convert.ToString(data.data);
                        string folderName = Guid.NewGuid().ToString();
                        string imageDir = Path.Combine(tempDir, folderName);
                        Pdf2JpgUtils.Pdf2Jpg(pdfFile, jpgFile, null, folderName);
                        //Pdf2JpgUtils.MergerImage(imageDir, targetPath);
                        /*
                        */
                        if (eventType == "PRINT")
                        {
                            //result = this.PreView(imageDir, eventType);
                            foreach (var file in Directory.GetFiles(imageDir))
                            {
                                this.PrintImage(file, printType);
                            }
                        }
                        else if (eventType == "PREVIEW")
                        {
                            result = this.PreView(imageDir, eventType);
                        }
                        try
                        {
                            //删除文件夹
                            Directory.Delete(imageDir, true);
                        }
                        catch (Exception ex)
                        {
                            result = string.Format("删除临时目录出错：{0};其他错误信息：{1}", imageDir, ex);
                        }
                        break;
                    case "HTML":
                        string tempPdf = Path.Combine(Directory.GetCurrentDirectory(), "tempPdf");
                        string folderName2 = Guid.NewGuid().ToString();
                        string tempPdfDir = Path.Combine(tempPdf, folderName2);
                        Directory.CreateDirectory(tempPdfDir);
                        string tempPdfPath = Path.Combine(tempPdfDir, "Html2Pdf.pdf");
                        Html2PdfUtils.Html2Pdf(Convert.ToString(data.data).Trim(), tempPdfPath);

                        string imageDir2 = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "tempImg"), folderName2);
                        Pdf2JpgUtils.Pdf2Jpg(tempPdfPath, Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "tempImg"), "Html2Pdf.jpg"), null, folderName2);
                        if (eventType == "PRINT")
                        {
                            foreach (var file in Directory.GetFiles(imageDir2))
                            {
                                this.PrintImage(file, printType);
                            }
                        }
                        else if (eventType == "PREVIEW")
                        {
                            result = this.PreView(imageDir2, eventType);
                        }
                        try
                        {
                            //删除文件夹
                            Directory.Delete(imageDir2, true);
                            Directory.Delete(tempPdfDir, true);
                        }
                        catch (Exception ex)
                        {
                            result = string.Format("删除临时目录出错：{0};其他错误信息：{1}", tempPdfDir, ex);
                        }
                        break;
                    default:
                        result = "参数异常:file_type字段只能为jpg、pdf、html";
                        break;
                }
            }
            catch(Exception ex)
            {
                result = "打印出错【" + ex.Message + "】";
            }
            Send(result);
        }

        protected override void OnOpen()
        {
            //Instance();
        }

        #region Html转换

        WebBrowser webBrowser = null;

        public void ConvertToImg()
        {
            webBrowser = new WebBrowser();

            //是否显式滚动条
            webBrowser.ScrollBarsEnabled = false;

            //加载HTML页面的地址
            webBrowser.Navigate("http://www.baidu.com");

            //页面加载完成执行事件
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);
        }

        private void webBrowser_DocumentCompleted(object sender, EventArgs e)//这个就是当网页载入完毕后要进行的操作
        {
            //获取解析后HTML的大小
            System.Drawing.Rectangle rectangle = webBrowser.Document.Body.ScrollRectangle;
            int width = rectangle.Width;
            int height = rectangle.Height;

            //设置解析后HTML的可视区域
            webBrowser.Width = width;
            webBrowser.Height = height;

            Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            webBrowser.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, width, height));

            //设置图片文件保存路径和图片格式，格式可以自定义
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "../../SaveFIle/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
        #endregion

    }
}

using System;
using System.IO;
using System.Drawing;
using WebSocketSharp;
using WebSocketSharp.Server;
using PrintControl.Model;
using PrintControl.Utils;
using PrintControl.Common;

namespace PrintControl.Services
{
    public class PrinterService : WebSocketBehavior
    {
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
        /// 客户端发送send请求会被触发，接收请求数据
        /// </summary>
        protected override void OnMessage(MessageEventArgs e)
        {
            String result = "打印成功！";
            try
            {
                MessageEvent<String> data = JsonHelper.DeserializeJsonToObject<MessageEvent<String>>(e.Data);
                string eventType = Convert.ToString(data.eventType).ToUpper();
                string printType = Convert.ToString(data.printType).ToUpper();
                string direction = Convert.ToString(data.direction);
                string printerName = Convert.ToString(data.printerName);
                string paperName = Convert.ToString(data.paperName);
                string folderName = Guid.NewGuid().ToString();  //pdf转图片所存放的文件夹名称
                string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "tempImg");  //生成图片临时存放文件夹
                string tempImageDir = Path.Combine(tempDir, folderName);
                //string targetPath = Path.Combine(tempDir, fileName + ".jpg");

                switch (Convert.ToString(data.fileType).ToUpper())
                {
                    case "JPG":
                        if (eventType == "PRINT")
                        {
                            result = PrintUtils.PrintImage(Convert.ToString(data.data), printType, direction, printerName, paperName);
                        }
                        else if (eventType == "PREVIEW")
                        {
                            result = printType == "FILEPATH" ? 
                                PrintUtils.PreView(Convert.ToString(data.data), eventType, direction, printerName, paperName)
                                : PrintUtils.PreViewByBase64(Convert.ToString(data.data), eventType, direction, printerName, paperName);
                        }
                        break;
                    case "TXT":
                        PrintUtils.PrintTxt(Convert.ToString(data.data), printType);
                        break;
                    case "PDF":
                        string fileName = Path.GetFileNameWithoutExtension(data.data);  //通过完整路径取得pdf文件名称作为jpg的文件名
                        Pdf2JpgUtils.Pdf2Jpg(Convert.ToString(data.data), Path.Combine(tempDir, fileName + ".jpg"), null, folderName);
                        if (eventType == "PRINT")
                        {
                            foreach (var file in Directory.GetFiles(tempImageDir))
                            {
                                result = PrintUtils.PrintImage(file, printType, direction, printerName, paperName);
                            }
                            try
                            {
                                //删除文件夹
                                Directory.Delete(tempImageDir, true);
                            }
                            catch (Exception ex)
                            {
                                result = string.Format("删除临时目录出错：{0};其他错误信息：{1}", tempImageDir, ex);
                            }
                        }
                        else if (eventType == "PREVIEW")
                        {
                            result = PrintUtils.PreView(tempImageDir, eventType, direction, printerName, paperName);
                        }
                        break;
                    case "HTML":
                        string tempPdf = Path.Combine(Directory.GetCurrentDirectory(), "tempPdf");
                        string tempPdfDir = Path.Combine(tempPdf, folderName);
                        Directory.CreateDirectory(tempPdfDir);
                        string tempPdfPath = Path.Combine(tempPdfDir, "Html2Pdf.pdf");
                        Html2PdfUtils.Html2Pdf(Convert.ToString(data.data).Trim(), tempPdfPath, paperName, direction);
                        Pdf2JpgUtils.Pdf2Jpg(tempPdfPath, Path.Combine(tempDir, "Html2Pdf.jpg"), null, folderName);
                        if (eventType == "PRINT")
                        {
                            foreach (var file in Directory.GetFiles(tempImageDir))
                            {
                                result = PrintUtils.PrintImage(file, printType, direction, printerName, paperName);
                            }
                            try
                            {
                                //删除临时文件夹
                                Directory.Delete(tempImageDir, true);
                                Directory.Delete(tempPdfDir, true);
                            }
                            catch (Exception ex)
                            {
                                result = string.Format("删除临时目录出错：{0};其他错误信息：{1}", tempPdfDir, ex);
                            }
                        }
                        else if (eventType == "PREVIEW")
                        {
                            result = PrintUtils.PreView(tempImageDir, eventType, direction, printerName, paperName);
                        }
                        break;
                    case "GETPRINTERNAME":
                        result = PrintUtils.GetPrinterNames();
                        break;
                    case "GETPAGESIZES":
                        result = PrintUtils.GetPageSizesByName(Convert.ToString(data.data));
                        break;
                    default:
                        result = "参数异常:fileType字段只能为jpg、pdf、html、getPrinterName、getPageSizes";
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
    }
}

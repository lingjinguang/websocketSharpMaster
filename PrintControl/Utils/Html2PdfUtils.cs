using System;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PrintControl.Common;

namespace PrintControl.Utils
{
    public static class Html2PdfUtils // : System.Web.UI.Page
    {
        public static void Html2Pdf(string htmlText, string tempPdfPath)
        {
            if (string.IsNullOrEmpty(htmlText))
                throw new Exception("传入的html无内容：" + htmlText);
            MemoryStream outputStream = new MemoryStream(); //实例化MemoryStream，用于存PDF 
            byte[] data = Encoding.UTF8.GetBytes(htmlText); //字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document();  //要写PDF的文件，建构子没填的话预设直式A4
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件预设开档时的缩放为100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //开启Document文件 
            doc.Open();

            //使用XMLWorkerHelper把Html parse到PDF档里
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8);

            //将pdfDest设定的资料写到PDF档
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);
            doc.Close();
            msInput.Close();
            outputStream.Close();
            //回传PDF档案 
            var bytes = outputStream.ToArray();

            var ret = Convert.ToBase64String(bytes);
            try
            {
                string strbase64 = ret;
                strbase64 = strbase64.Replace(' ', '+');
                System.IO.MemoryStream stream = new System.IO.MemoryStream(Convert.FromBase64String(strbase64));
                System.IO.FileStream fs = new System.IO.FileStream(tempPdfPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                byte[] b = stream.ToArray();
                //byte[] b = stream.GetBuffer();
                fs.Write(b, 0, b.Length);
                fs.Close();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
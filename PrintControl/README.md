**PrintControl项目**：基于websocket-shap（https://github.com/sta/websocket-sharp）开源项目开发，适配于xp系统。

**项目结构图**

![image-20200728100258687](C:\Users\1\AppData\Roaming\Typora\typora-user-images\image-20200728100258687.png)

Common文件夹：放置一些公共文件。

Model文件夹：放置数据类型的cs文件。

Services文件夹：目前只有一个PrintServices.cs（只支持jpg、html、pdf打印）。

Test文件夹：print.html打印测试文件（ws的创建、连接、数据传输）。

Utils文件夹：工具文件夹，工具属性的方法另外封装起来。

MainForm.cs：应用的主图形界面。

Program.cs：程序的入口Main方法

**后端搭建WebSocket服务**（MainForm.cs）

```C#
引用
using WebSocketSharp.Net;
using WebSocketSharp.Server;

        private Setting setting = null;
        HttpServer httpsv = null;
        private void StartService(bool isInit)
        {
            try
            {
                if (httpsv != null) httpsv.Stop();
                httpsv = null;

                //默认保存端口
                setting.Port = this.txt_port.Text;
                FileUtils.SaveSetting(setting);

                //监听服务开启
                int post = Convert.ToInt32(this.txt_port.Text);
                httpsv = new HttpServer(post);
                
                // 添加WebSocket服务
                httpsv.AddWebSocketService<PrinterService>("/PrinterService");
				
                //开启服务
                httpsv.Start();
                string txt = httpsv.IsListening ? "服务运行中..." : "启动失败";
                this.lab_ServiceState.Text = txt;
            }
            catch (Exception ex)
            {
                this.lab_ServiceState.Text = "失败：可能端口已被占用." + ex.Message;
            }
        }
```

**前端连接webSocket服务**

```js
			var ws;
			function connectWebSocket() {
                //实例化WebSocket对象
                ws = new WebSocket($("#uri").val());
                //连接成功建立后响应
                ws.onopen = function () {
                    log("成功连接到" + $("#uri").val());
                }
                //收到服务器消息后响应
                ws.onmessage = function (event) {
                    log("收到服务器消息:"+event.data);
                    var ret = JSON.parse(event.data);
                    if (ret.code != 0)
                        log("异常：" + ret.msg);
                    else
                        log("打印成功，后续需要回调打印成功接口");
                }
                //连接关闭后响应
                ws.onclose = function () {
                    log("关闭连接");
                    $("#disconnect").attr({ "disabled": "disabled" });
                    $("#uri").removeAttr("disabled");
                    $("#connect").removeAttr("disabled");
                    ws = null;
                }
                return false;
            }
			//发生信息到webSocket服务器进行通信
            $("#printPdfByUrl").click(function () {
                if (!ws || ws.readyState !== 1) {
                    alert('请先连接服务');
                    return false;
                }
                var url = $('#pdfUrl').val();
                var data = url;
                var sendData = {
                    file_type: "pdf",
                    print_type: "filePath",
                    event_type: "print",
                    data: data
                };
                
                var urlArr = [url];
                //发送数据
                //测试打印多张图片
                for(var i = 0; i < 1; i++){
                    for(var j = 0; j < urlArr.length; j++){
                        sendData.data = urlArr[j]; 
                        ws.send(JSON.stringify(sendData));
                    }
                }
            });

```

**数据类型**

```js
var sendData = {
    file_type: "pdf",//jpg、html、pdf 三种文件类型
    print_type: "filePath",//filePath、Base64 二种打印数据来源方式类型
    event_type: "print",//这个其他类型暂时没用
    data: data
};
```

**后端响应前端的send事件**（OnMessage）

```c#

/// <summary>
/// 客户端发送send请求会被触发，接收请求数据
/// </summary>
protected override void OnMessage(MessageEventArgs e)
{
    String result = "打印成功！";
    try
    {
        string dataStr = e.Data;
        var data = JsonHelper.DeserializeJsonToObject<MessageEvent<String>>(e.Data);
        switch (Convert.ToString(data.file_type).ToUpper())
        {
            case "JPG":
                PrintImage(Convert.ToString(data.data), Convert.ToString(data.print_type).ToUpper());
                break;
            case "TXT":
                PrintTxt(Convert.ToString(data.data), Convert.ToString(data.print_type).ToUpper());
                break;
            case "PDF":                     
                string tempDir = Path.Combine(Directory.GetCurrentDirectory(), "tempImg"); 
                string fileName = Path.GetFileNameWithoutExtension(data.data); //通过完整路径取得pdf文件爱你名称作为jpg的文件名
                string jpgFile = Path.Combine(tempDir, fileName + ".jpg");
                string pdfFile = Convert.ToString(data.data);
                string folderName = Guid.NewGuid().ToString();
                string imageDir = Path.Combine(tempDir, folderName);
                Pdf2JpgUtils.Pdf2Jpg(pdfFile, jpgFile, null, folderName);
                foreach (var file in Directory.GetFiles(imageDir))
                {
                    PrintImage(file, Convert.ToString(data.print_type).ToUpper());
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
                foreach (var file in Directory.GetFiles(imageDir2))
                {
                    PrintImage(file, Convert.ToString(data.print_type).ToUpper());
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

```

**JPG打印**

```C#
//使用Drawing进行图片处理
using System.Drawing;
using System.Drawing.Printing;
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

            //e.Graphics.DrawImage(image数据, margin起始位置x, margin起始位置y, imgWidth, imgHeight);
            //e.Graphics.DrawImage(image, m);
            /* 小于等于4961*7016的正常
                    Point loc = new Point(10, 10);
                    e.Graphics.DrawImage(image, loc);
                    */
        };
        //开始打印
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
/// Base64转成Imgage数据类型
/// </summary>
private Image Base64ToImg(string base64str)
{
    byte[] arr = Convert.FromBase64String(base64str);
    MemoryStream ms = new MemoryStream(arr);
    Bitmap bmp = new Bitmap(ms);
    return bmp;
}
```

**PDF打印（PDF转成JPG在通过PrintImage打印）**

```C#
应用控件 PdfiumViewer处理pdf转成pdf文档
    
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
                        imgScale.Save(targetPath);
                    }
                }
            }
            else
            {
                image.Save(Path.Combine(imageDir, folderName + ".jpg"));
                image.Save(targetPath);
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

//var image = pdfDocument.Render(pages.Value, ConfigDefine.Pdf2JpgResolution, ConfigDefine.Pdf2JpgResolution, PdfiumViewer.PdfRenderFlags.CorrectFromDpi)
pdf转成Image类型数据，接下来进行
```

**HTML打印（HTML转PDF转图片打印）**

```C#
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;
using iTextSharp.tool.xml;

namespace PrintControl.Utils
{
    public static class Html2PdfUtils // : System.Web.UI.Page
    {
        public static void Html2Pdf(string htmlText, string tempPdfPath)
        {
            if (string.IsNullOrEmpty(htmlText))
                throw new Exception("传入的html无内容：" + htmlText);
            MemoryStream outputStream = new MemoryStream();//要把PDF写到哪个串流
            byte[] data = Encoding.UTF8.GetBytes(htmlText);//字串转成byte[]
            MemoryStream msInput = new MemoryStream(data);
            Document doc = new Document();//要写PDF的文件，建构子没填的话预设直式A4
            PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
            //指定文件预设开档时的缩放为100%
            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
            //开启Document文件 
            doc.Open();

            //使用XMLWorkerHelper把Html parse到PDF档里
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontFactory());
            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8,字体);

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
```


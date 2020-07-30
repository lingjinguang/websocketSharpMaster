using System;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using PrintControl.Model;
using PrintControl.Utils;
using PrintControl.Services;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace PrintControl
{
    public partial class MainForm : Form
    {
        private Setting setting = null;
        HttpServer httpsv = null;
        public MainForm()
        {
            InitializeComponent();
            setting = FileUtils.GetSetting();//初始化默认配置信息

            //显示默认端口
            this.txt_port.Text = setting == null || setting.Port == null ? "4649" : setting.Port;
            this.StartService(true);


            //初始化打印列表
            InitPrinterList();
        }
        
        /// <summary>
        /// 服务启动
        /// </summary>
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
                //AppSocketServer.GetInstance(txt_port.Text, true).Start();
                int post = Convert.ToInt32(this.txt_port.Text);
                httpsv = new HttpServer(post);

                httpsv.DocumentRootPath = "../../Public";//ConfigurationManager.AppSettings["DocumentRootPath"];

                // Set the HTTP GET request event.
                httpsv.OnGet += (sender, e) =>
                {
                    var req = e.Request;
                    var res = e.Response;

                    var path = req.RawUrl;
                    if (path == "/")
                        path += "index.html";

                    byte[] contents;
                    if (!e.TryReadFile(path, out contents))
                    {
                        res.StatusCode = (int)HttpStatusCode.NotFound;
                        return;
                    }

                    if (path.EndsWith(".html"))
                    {
                        res.ContentType = "text/html";
                        res.ContentEncoding = Encoding.UTF8;
                    }
                    else if (path.EndsWith(".js"))
                    {
                        res.ContentType = "application/javascript";
                        res.ContentEncoding = Encoding.UTF8;
                    }

                    res.ContentLength64 = contents.LongLength;
                    res.Close(contents, true);
                };

                // 添加WebSocket服务
                httpsv.AddWebSocketService<Echo>("/Echo");
                httpsv.AddWebSocketService<Chat>("/Chat");
                httpsv.AddWebSocketService<PrinterService>("/PrinterService");

                httpsv.Start();
                string txt = httpsv.IsListening ? "服务运行中..." : "启动失败";
                this.lab_ServiceState.Text = txt;
            }
            catch (Exception ex)
            {
                this.lab_ServiceState.Text = "失败：可能端口已被占用." + ex.Message;
            }
        }
        /// <summary>
        /// 刷新打印机列表
        /// </summary>
        private void link_Refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            InitPrinterList();
        }
        /*
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            setting.Name = this.comboBox1.Text;
            setting.IsDefault = true;
            Boolean result = FileUtils.SaveSetting(setting);
            if (result)
            {
                this.lab_defualtPrinterName.Text = setting.Name;
                if (SetDefaultPrinter(this.comboBox1.Text)) //设置默认打印机
                {
                    MessageBox.Show("设置为默认打印机成功！");
                }
                else
                {
                    MessageBox.Show("设置为默认打印机失败！");
                }
            }
            else
                MessageBox.Show("系统错误,请重试!");
        }
        */
        /// <summary>
        /// 初始化打印机列表 如果有默认的选中默认
        /// </summary>
        private void InitPrinterList()
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)       //获取当前打印机
            {
                comboBox1.Items.Add(PrinterSettings.InstalledPrinters[i]);
                if (setting != null && setting.Name != null && setting.Name.Equals(PrinterSettings.InstalledPrinters[i]))
                {
                    //显示默认打印机名称
                    comboBox1.SelectedIndex = i;
                    this.lab_defualtPrinterName.Text = setting.Name ?? "无";
                }
            }
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);
        /// <summary>
        /// 重新启动
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.lab_ServiceState.Text = "正在重启...";
            StartService(true);
        }
        /// <summary>
        /// 设置默认打印机
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            setting.Name = this.comboBox1.Text;
            setting.IsDefault = true;
            Boolean result = FileUtils.SaveSetting(setting);
            if (result)
            {
                this.lab_defualtPrinterName.Text = setting.Name;
                if (SetDefaultPrinter(this.comboBox1.Text)) //设置默认打印机
                {
                    MessageBox.Show("设置为默认打印机成功！");
                }
                else
                {
                    MessageBox.Show("设置为默认打印机失败！");
                }
            }
            else
                MessageBox.Show("系统错误,请重试!");
        }
    }
}

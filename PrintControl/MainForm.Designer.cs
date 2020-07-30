namespace PrintControl
{
    //partial class MainForm
    //{
    //    /// <summary>
    //    /// 必需的设计器变量。
    //    /// </summary>
    //    private System.ComponentModel.IContainer components = null;

    //    /// <summary>
    //    /// 清理所有正在使用的资源。
    //    /// </summary>
    //    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing && (components != null))
    //        {
    //            components.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    #region Windows 窗体设计器生成的代码

    //    /// <summary>
    //    /// 设计器支持所需的方法 - 不要
    //    /// 使用代码编辑器修改此方法的内容。
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        this.components = new System.ComponentModel.Container();
    //        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    //        this.Text = "Form1";
    //    }

    //    #endregion
    //}



    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.mymenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.link_Refresh = new System.Windows.Forms.LinkLabel();
            this.lab_defualtPrinterName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lab_readCard = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.llbScanningRefresh = new System.Windows.Forms.LinkLabel();
            this.btn_setCapture = new System.Windows.Forms.Button();
            this.lab_currentCapture = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.combo_capture = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lab_ServiceState = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mymenu.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(433, 141);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "设 置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownHeight = 120;
            this.comboBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.ItemHeight = 14;
            this.comboBox1.Location = new System.Drawing.Point(85, 142);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(331, 22);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(46, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择打印机（        ）：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(362, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 31);
            this.button2.TabIndex = 4;
            this.button2.Text = "重新启动";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 9;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.mymenu;
            this.notifyIcon1.Text = "云PACS工具集";
            this.notifyIcon1.Visible = true;
            // 
            // mymenu
            // 
            this.mymenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.mymenu.Name = "mymenu";
            this.mymenu.Size = new System.Drawing.Size(95, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem1.Text = "退出";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 134);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(581, 257);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.link_Refresh);
            this.tabPage1.Controls.Add(this.lab_defualtPrinterName);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(573, 224);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "打印服务";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // link_Refresh
            // 
            this.link_Refresh.AutoSize = true;
            this.link_Refresh.Location = new System.Drawing.Point(143, 95);
            this.link_Refresh.Name = "link_Refresh";
            this.link_Refresh.Size = new System.Drawing.Size(65, 20);
            this.link_Refresh.TabIndex = 10;
            this.link_Refresh.TabStop = true;
            this.link_Refresh.Text = "刷新列表";
            // 
            // lab_defualtPrinterName
            // 
            this.lab_defualtPrinterName.AutoSize = true;
            this.lab_defualtPrinterName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_defualtPrinterName.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lab_defualtPrinterName.Location = new System.Drawing.Point(181, 47);
            this.lab_defualtPrinterName.Name = "lab_defualtPrinterName";
            this.lab_defualtPrinterName.Size = new System.Drawing.Size(72, 16);
            this.lab_defualtPrinterName.TabIndex = 9;
            this.lab_defualtPrinterName.Text = "请设置！";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(82, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "当前打印机：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lab_readCard);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(573, 224);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "读卡服务";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lab_readCard
            // 
            this.lab_readCard.AutoSize = true;
            this.lab_readCard.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_readCard.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lab_readCard.Location = new System.Drawing.Point(155, 87);
            this.lab_readCard.Name = "lab_readCard";
            this.lab_readCard.Size = new System.Drawing.Size(44, 25);
            this.lab_readCard.TabIndex = 0;
            this.lab_readCard.Text = "----";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.llbScanningRefresh);
            this.tabPage3.Controls.Add(this.btn_setCapture);
            this.tabPage3.Controls.Add(this.lab_currentCapture);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.combo_capture);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(573, 224);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "扫描服务";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // llbScanningRefresh
            // 
            this.llbScanningRefresh.AutoSize = true;
            this.llbScanningRefresh.Location = new System.Drawing.Point(152, 94);
            this.llbScanningRefresh.Name = "llbScanningRefresh";
            this.llbScanningRefresh.Size = new System.Drawing.Size(65, 20);
            this.llbScanningRefresh.TabIndex = 13;
            this.llbScanningRefresh.TabStop = true;
            this.llbScanningRefresh.Text = "刷新列表";
            // 
            // btn_setCapture
            // 
            this.btn_setCapture.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_setCapture.Location = new System.Drawing.Point(413, 136);
            this.btn_setCapture.Name = "btn_setCapture";
            this.btn_setCapture.Size = new System.Drawing.Size(75, 25);
            this.btn_setCapture.TabIndex = 12;
            this.btn_setCapture.Text = "设 置";
            this.btn_setCapture.UseVisualStyleBackColor = true;
            // 
            // lab_currentCapture
            // 
            this.lab_currentCapture.AutoSize = true;
            this.lab_currentCapture.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_currentCapture.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lab_currentCapture.Location = new System.Drawing.Point(195, 49);
            this.lab_currentCapture.Name = "lab_currentCapture";
            this.lab_currentCapture.Size = new System.Drawing.Size(72, 16);
            this.lab_currentCapture.TabIndex = 11;
            this.lab_currentCapture.Text = "请设置！";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(84, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 10;
            this.label8.Text = "当前视频设备：";
            // 
            // combo_capture
            // 
            this.combo_capture.DropDownHeight = 120;
            this.combo_capture.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.combo_capture.FormattingEnabled = true;
            this.combo_capture.IntegralHeight = false;
            this.combo_capture.ItemHeight = 14;
            this.combo_capture.Location = new System.Drawing.Point(87, 139);
            this.combo_capture.Name = "combo_capture";
            this.combo_capture.Size = new System.Drawing.Size(302, 22);
            this.combo_capture.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(42, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(208, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "选择视频设备（       ）：";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.groupBox1.Controls.Add(this.lab_ServiceState);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_port);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(581, 128);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通讯连接";
            // 
            // lab_ServiceState
            // 
            this.lab_ServiceState.AutoSize = true;
            this.lab_ServiceState.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_ServiceState.Location = new System.Drawing.Point(244, 77);
            this.lab_ServiceState.Name = "lab_ServiceState";
            this.lab_ServiceState.Size = new System.Drawing.Size(118, 21);
            this.lab_ServiceState.TabIndex = 10;
            this.lab_ServiceState.Text = "服务正在启动...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(160, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 21);
            this.label5.TabIndex = 7;
            this.label5.Text = "运行状态：";
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(248, 36);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(108, 26);
            this.txt_port.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(160, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "通讯端口：";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 391);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "医网云PACS - 服务程序";
            this.mymenu.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip mymenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lab_defualtPrinterName;
        private System.Windows.Forms.LinkLabel link_Refresh;
        private System.Windows.Forms.Label lab_ServiceState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox combo_capture;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lab_currentCapture;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_setCapture;
        private System.Windows.Forms.Label lab_readCard;
        private System.Windows.Forms.LinkLabel llbScanningRefresh;
    }
}


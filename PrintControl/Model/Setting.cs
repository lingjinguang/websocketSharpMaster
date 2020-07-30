using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace PrintControl.Model
{
    /// <summary>
    /// 应用程序配置参数
    /// </summary>
    [Serializable]
    public class Setting
    {
        private string _name;
        private bool _isDefault = false;
        private string _port;
        private string _moniker;
        private string _videoName;

        public Setting()
        { }

        public Setting(string name)
        {
            Name = name;
        }
        /*
        /// <summary>
        /// 默认打印机名称
        /// </summary>
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get => _isDefault; set => _isDefault = value; }
        /// <summary>
        /// 通讯端口
        /// </summary>
        public string Port { get => _port; set => _port = value; }
        /// <summary>
        /// 视频设备
        /// </summary>
        public string MonikerString { get => _moniker; set => _moniker = value; }
        /// <summary>
        /// 视频设备名称
        /// </summary>
        public string VideoName { get => _videoName; set => _videoName = value; }
        */

        
        /// <summary>
        /// 默认打印机名称
        /// </summary>
        public string Name { get ; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get ; set ; }
        /// <summary>
        /// 通讯端口
        /// </summary>
        public string Port { get ; set ; }
        /// <summary>
        /// 视频设备
        /// </summary>
        public string MonikerString { get; set ; }
        /// <summary>
        /// 视频设备名称
        /// </summary>
        public string VideoName { get ; set ; }
    }
}

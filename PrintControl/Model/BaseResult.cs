using PrintControl.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace PrintControl.Model
{
    /// <summary>
    /// 程序处理结果
    /// </summary>
    public class BaseResult
    {
        public const int success_code = 0;
        public const int error_code = -1;

        /// <summary>
        /// 返回处理代码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回处理信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 返回处理成功
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static BaseResult Success(string msg )
        {
            return new BaseResult()
            {
                code = success_code,
                msg = msg
            };
        }

        /// <summary>
        /// 返回处理失败
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static BaseResult Error(string msg)
        {
            return new BaseResult()
            {
                code = error_code,
                msg = msg
            };
        }

        public static BaseResult Create(int code, string msg)
        {
            return new BaseResult()
            {
                code = code,
                msg = msg
            };
        }

        /// <summary>
        /// 判断是否为错误结果
        /// </summary>
        /// <returns></returns>
        public bool IsError()
        {
            return this.code != BaseResult.success_code;
        }

        /// <summary>
        /// 判断是否为正确处理结果
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return this.code == BaseResult.success_code;
        }

        /// <summary>
        /// 返回处理结果
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonHelper.SerializeObject(this);
        }
    }
}

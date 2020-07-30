using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace PrintControl.Model
{
    public class PrinterData
    {
        ///// <summary>
        ///// 医院名称
        ///// </summary>
        //public string HospitalName { get; set; }

        ///// <summary>
        ///// 标题
        ///// </summary>
        //public string Title { get; set; }

        ///// <summary>
        ///// 检查号
        ///// </summary>
        //public string AccessionNumber { get; set; }

        ///// <summary>
        ///// 姓名
        ///// </summary>
        //public string Name { get; set; }

        ///// <summary>
        ///// 性别
        ///// </summary>
        //public string Sex { get; set; }

        ///// <summary>
        ///// 年龄
        ///// </summary>
        //public string Age { get; set; }

        ///// <summary>
        ///// 检查时间
        ///// </summary>
        //public DateTime? ObservationDate { get; set; }

        ///// <summary>
        ///// 申请科室
        ///// </summary>
        //public string RequestDeptName { get; set; }

        ///// <summary>
        ///// 病历号
        ///// </summary>
        //public string MedRecNO { get; set; }

        ///// <summary>
        ///// 病床号
        ///// </summary>
        //public string InPatientBed { get; set; }

        ///// <summary>
        ///// 所见
        ///// </summary>
        //public string Sight { get; set; }

        ///// <summary>
        ///// 诊断
        ///// </summary>
        //public string Diagnosis { get; set; }

        ///// <summary>
        ///// 报告医生
        ///// </summary>
        //public string ResultAssistantName { get; set; }

        ///// <summary>
        ///// 审核医生
        ///// </summary>
        //public string ResultPrincipalName { get; set; }

        ///// <summary>
        ///// 报告时间
        ///// </summary>
        //public DateTime ReportTime { get; set; }

        public object PrintContent { get; set; }

        public string PrintName { get; set; }
        /// <summary>
        /// 模板数据,base64
        /// </summary>
        public string TemplateContent { get; set; }
    }
}

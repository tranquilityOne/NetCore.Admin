// File:    RMS_Buttons.cs
// Author:  Administrator
// Created: 2016年7月22日 星期五 17:53:02
// Purpose: Definition of Class RMS_Buttons


using Core.PetaPocoServer;
using System;
namespace Web.Sys.Model.RMS
{
    /// RMS_Buttons按钮
    [TableName("rms_buttons")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_buttons 
    {

        public Int32 id { get; set; }
        public string buttonname
        {
            set;
            get;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public Int32? orderno
        {
            set;
            get;
        }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon
        {
            set;
            get;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks
        {
            set;
            get;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime
        {
            set;
            get;
        }
        public DateTime? modifytime
        {
            set;
            get;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string valuename
        {
            set;
            get;
        }
        /// <summary>
        /// 方法
        /// </summary>
        public string functionname
        {
            set;
            get;
        }

    }
}
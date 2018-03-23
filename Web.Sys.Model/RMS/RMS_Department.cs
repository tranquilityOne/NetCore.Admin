using System;
using System.Collections.Generic;
using System.Text;
using Core.PetaPocoServer;
namespace Web.Sys.Model
{
    [TableName("rms_department")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_department
    {
        public int id { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public int parentid
        {
            get;
            set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int orderno
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime createtime
        {
            get;
            set;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime modifytime
        {
            get;
            set;
        }

        /// <summary>
        /// 公司
        /// </summary>
        public int companyid
        {
            get;
            set;
        }

    }
}

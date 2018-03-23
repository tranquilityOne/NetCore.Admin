using System;
using System.Collections.Generic;
using System.Text;
using Core.PetaPocoServer;

namespace Web.Sys.Model
{
    [TableName("rms_user")]
    [PrimaryKey("id", autoIncrement = false)]
    public class rms_user
    {
        public string id { get; set; }
        /// <summary>
        /// 部门主键
        /// </summary>
        public int departmentid
        {
            get;
            set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        public string loginname
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string truename
        {
            get;
            set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string password
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
        /// 修改时间
        /// </summary>
        public DateTime modifytime
        {
            get;
            set;
        }

        /// <summary>
        /// 用户类型 为1正常 姓名登录为0，身份证为2
        /// </summary>
        public Int32? usertype
        {
            get;
            set;
        }

        /// <summary>
        /// 手机
        /// </summary>
        public string phone
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

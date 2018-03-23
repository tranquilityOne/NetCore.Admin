using Core.PetaPocoServer;
using System;

namespace Web.Sys.Model
{
    [TableName("v_userrole")]
    [PrimaryKey("id", autoIncrement = false)]
    public partial class v_userrole
    {

        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string loginname
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string truename
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string password
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? createtime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? modifytime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int departmentid
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string roleid
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string rolename
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 usertype
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string phone
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string departmentname
        {
            get;
            set;
        }

        /// <summary>
        /// 权限 的json数据
        /// </summary>
        public string jurisdiction
        {
            get;
            set;
        }

        /// <summary>
        /// 当前公司
        /// </summary>
        public int companyid
        {
            get;
            set;

        }

        /// <summary>
        /// 当前公司
        /// </summary>
        public string companyName
        {
            get;
            set;

        }
    }
}

using System;
using Core.PetaPocoServer;
namespace Web.Sys.Model
{
    [TableName("rms_role")]
    [PrimaryKey("id", autoIncrement = false)]

    public class rms_role
    {

        public string id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string rolename
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
        /// 创建人
        /// </summary>
        public string createby
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string modifyby
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
        /// 角色类型（1管理员）
        /// </summary>
        public Int16 roletypes
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
        /// 公司
        /// </summary>
        public int companyid
        {
            get;
            set;

        }

        /// <summary>
        /// 项目
        /// </summary>
        public int projectid
        {
            get;
            set;

        }
    }
}

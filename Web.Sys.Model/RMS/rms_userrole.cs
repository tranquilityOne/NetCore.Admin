using System;
using System.Collections.Generic;
using System.Text;
using Core.PetaPocoServer;

namespace Web.Sys.Model
{
    ///RMS_UserRole用户所属角色表
    [TableName("rms_userrole")]
    [PrimaryKey("id", autoIncrement = false)]
    public class rms_userrole
    {

        public string id { get; set; }
        /// <summary>
        /// 用户的id
        /// </summary>
        public string userid
        {
            get;
            set;
        }

        /// <summary>
        /// 角色的id
        /// </summary>
        public string roleid
        {
            get;
            set;
        }
    }
}

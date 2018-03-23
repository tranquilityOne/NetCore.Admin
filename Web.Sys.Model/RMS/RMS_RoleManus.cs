// File:    RMS_RoleManus.cs
// Author:  Administrator
// Created: 2016年7月22日 星期五 17:53:02
// Purpose: Definition of Class RMS_RoleManus


using Core.PetaPocoServer;
using System;
namespace Web.Sys.Model.RMS
{
    /// RMS_RoleManus角色菜单
    [TableName("rms_rolemanus")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_rolemanus 
    {

        public Int32 id { get; set; }

        /// <summary>
        /// 角色的id
        /// </summary>
        public string roleid
        {
            get;
            set;
        }



        /// <summary>
        /// 菜单的id
        /// </summary>
        public int manuid
        {
            get;
            set;
        }

    }
}
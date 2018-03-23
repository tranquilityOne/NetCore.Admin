// File:    RMS_RoleManuButtons.cs
// Author:  Administrator
// Created: 2016年7月22日 星期五 17:53:02
// Purpose: Definition of Class RMS_RoleManuButtons


using Core.PetaPocoServer;
using System;
namespace Web.Sys.Model.RMS
{
    /// RMS_RoleManuButtons角色菜单的按钮表
    [TableName("rms_rolemanubuttons")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_rolemanubuttons 
    {
        public int id { get; set; }
        /// <summary>
        /// 角色的id
        /// </summary>
        public string roleid
        {
            get;
            set;
        }



        /// <summary>
        /// 菜单的按钮的id
        /// </summary>
        public string menubuttonsid
        {
            get;
            set;
        }

    }
}
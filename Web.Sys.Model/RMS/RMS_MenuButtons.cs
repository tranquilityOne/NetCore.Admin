// File:    RMS_MenuButtons.cs
// Author:  Administrator
// Created: 2016年7月22日 星期五 17:53:02
// Purpose: Definition of Class RMS_MenuButtons


using Core.PetaPocoServer;
using System;
namespace Web.Sys.Model.RMS
{
    /// RMS_MenuButtons菜单的按钮
    [TableName("rms_menubuttons")]
    [PrimaryKey("id", autoIncrement = true)]
    public class rms_menubuttons 
    {

        public int id { get; set; }
        /// <summary>
        /// 按钮的id
        /// </summary>
        public int buttonid
        {
            get;
            set;
        }



        /// <summary>
        /// 菜单id
        /// </summary>
        public int manuid
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

    }
}
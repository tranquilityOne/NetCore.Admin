// File:    RMS_Menus.cs
// Author:  Administrator
// Created: 2016年7月22日 星期五 17:53:02
// Purpose: Definition of Class RMS_Menus


using Core.PetaPocoServer;
using System;
namespace Web.Sys.Model.RMS
{
    /// v_rms_menus菜单
    [TableName("v_rms_menus")]
    [PrimaryKey("id", autoIncrement = true)]
    public class v_rms_menus
    {
        public Int32 id { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public int parentmanuid
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
        public string manuname
        {
            get;
            set;
        }



        /// <summary>
        /// 跳转地址
        /// </summary>
        public string url
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
        /// 大图标
        /// </summary>
        public string micon
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
        /// 是否可用
        /// </summary>
        public bool isenable
        {
            get;
            set;
        }



        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isshow
        {
            get;
            set;
        }
        /// <summary>
        /// 类型 0顶级 1集合（有子集 当包用） 2 连接（带地址 提供跳转功能）
        /// </summary>
        public Int16 typeid
        {
            get;
            set;

        }
        /// <summary>
        /// 值、编号
        /// </summary>
        public string valuename
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
        /// <summary>
        /// 项目名
        /// </summary>
        public string projectname
        {
            get;
            set;

        }

    }
}
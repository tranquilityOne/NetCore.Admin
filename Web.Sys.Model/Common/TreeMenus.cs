using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Sys.Model.Common
{

    /// <summary>
    /// 数型菜单数据
    /// </summary>
    public class TreeMenus
    {
        public int Id
        {
            get;
            set;
        }
        public int ParentManuId
        {
            get;
            set;
        }
        public Int32? OrderNo
        {
            get;
            set;
        }
        public String Name
        {
            get;
            set;
        }
        public String URL
        {
            get;
            set;
        }
        public String iconCls
        {
            get;
            set;
        }
        public String MIcon
        {
            get;
            set;
        }

        public String Remarks
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
        /// 项目编号
        /// </summary>
        public string projectname
        {
            get;
            set;
        }
        public List<TreeMenus> children { get; set; }

    }
}

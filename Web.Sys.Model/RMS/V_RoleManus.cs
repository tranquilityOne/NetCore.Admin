using Core.PetaPocoServer;
using System;


namespace Web.Sys.Model.RMS
{

    [TableName("v_rolemanus")]
    [PrimaryKey("id", autoIncrement = false)]
    public class v_rolemanus
    {
        public int manuid
        {
            get;
            set;
        }
        public string roleid
        {
            get;
            set;
        }
        public string parentmanuid
        {
            get;
            set;
        }
        public string remarks
        {
            get;
            set;
        }
        public Int32? orderno
        {
            get;
            set;
        }
        public string manuname
        {
            get;
            set;
        }
        public string url
        {
            get;
            set;
        }
        public Boolean? isshow
        {
            get;
            set;
        }
        public Boolean? isenable
        {
            get;
            set;
        }
        public string icon
        {
            get;
            set;
        }
        public string micon
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
    }


}

using System;
using Core.PetaPocoServer;

namespace Web.Sys.Model.RMS 
{

    /// <summary>
    /// 角色菜单的按钮表
    /// </summary>
    [TableName("v_rolemanubuttons")]
    [PrimaryKey("id", autoIncrement = false)]
    public class v_rolemanubuttons 
    {
        public int id { get; set; }
        public string roleid
        {
            get;
            set;
        }
        public int manuid
        {
            get;
            set;
        }
        public Int32? orderno
        {
            get;
            set;
        }
        public string icon
        {
            get;
            set;
        }
        public string remarks
        {
            get;
            set;
        }
        public DateTime? createtime
        {
            get;
            set;
        }
        public DateTime? modifytime
        {
            get;
            set;
        }
        public string valuename
        {
            get;
            set;
        }
        public string functionname
        {
            get;
            set;
        }
        public string buttonname
        {
            get;
            set;
        }

    }

}

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Core.PetaPocoServer;

namespace Web.Sys.Model.RMS 
{
    /// <summary>
    /// 菜单按钮
    /// </summary>
    [TableName("v_menubuttons")]
    [PrimaryKey("id", autoIncrement = false)]
    public class v_menubuttons 
    {
        public int id { get; set; }
        public int manuid
        {
            get;
            set;
        }
        public string menubuttonsid
        {
            get;
            set;
        }
        public Int32? orderno
        {
            get;
            set;
        }

        public string buttonname
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




    }


}

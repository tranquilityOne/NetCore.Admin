using System;
using System.Collections.Generic;
using System.Text;
using Core.Common;
using Core.PetaPocoServer;

namespace Web.Sys.Model
{  
        /// <summary>
        /// 个人信息
        /// </summary>
        public class AdminUserInfo
        {
            /// <summary>
            /// 用户Id
            /// </summary>
            public string Id
            {
                get;
                set;

            }
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName
            {
                get;
                set;
            }
            /// <summary>
            /// 密码
            /// </summary>
            public string Password
            {
                get;
                set;
            }
            /// <summary>
            /// 部门
            /// </summary>
            public int DepartmentId
            {
                get;
                set;

            }
            /// <summary>
            /// 当前公司
            /// </summary>
            public int companyid
            {
                get;
                set;

            }
            /// <summary>
            /// 当前公司
            /// </summary>
            public string companyName
            {
                get;
                set;

            }

            /// <summary>
            /// 部门单位
            /// </summary>
            public string DepartmentName
            {
                get;
                set;

            }
            /// <summary>
            /// 角色Id
            /// </summary>
            public string RoleId
            {
                get;
                set;
            }
            /// <summary>
            /// 用户类型
            /// </summary>
            public int UserTypes
            {
                get;
                set;

            }

            /// <summary>
            /// 拥有的菜单
            /// </summary>
            public Dictionary<string, Manu> ListManusD
            {
                get;
                set;
            }
        }
        /// <summary>
        /// 用户类型
        /// </summary>
        public enum UserType
        {
            admin = 0,//管理员
            name = 1,//姓名登录
            idCad = 1,//身份证登录
        }
        /// <summary>
        ///菜单
        /// </summary>
        public class Manu
        {
            public m_v_rolemanus manuinfo
            {
                get;
                set;
            }

            /// <summary>
            /// 当前角色 没拥有的按钮  
            /// </summary>
            public Dictionary<string, m_v_menubuttons> nobuttonD
            {
                get;
                set;
            }


            /// <summary>
            ///当前角色 拥有的按钮
            /// </summary>
            public Dictionary<string, m_v_rolemanubuttons> havebuttonsD
            {
                get;
                set;
            }
        }


        #region 再定义 去掉不用的
        [TableName("v_rolemanus")]
        [PrimaryKey("id", autoIncrement = false)]
        public class m_v_rolemanus
        {
            public int manuid
            {
                get;
                set;
            }
          
            public int parentmanuid
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


        /// <summary>
        /// 这个角色 没拥有 这个菜单的 按钮
        /// </summary>
        [TableName("v_menubuttons")]
        [PrimaryKey("id", autoIncrement = false)]
        public class m_v_menubuttons
        {
           
            public int manuid
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

        /// <summary>
        /// 这个角色 拥有 这个菜单的 按钮
        /// </summary>
        [TableName("v_rolemanubuttons")]
        [PrimaryKey("id", autoIncrement = false)]
        public class m_v_rolemanubuttons
        {
           
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
        #endregion
    
}

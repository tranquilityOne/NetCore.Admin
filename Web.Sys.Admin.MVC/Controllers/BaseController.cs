using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.Model;
using Web.Sys.Admin.MVC.Extensions;
using Core.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using e3net.Common;

namespace Web.Sys.Admin.MVC.Controllers
{
    public class BaseController : Controller
    {
        public AdminUserInfo UserData
        {
            set { HttpContext.Session.Set<AdminUserInfo>("UserData", value); }
            get
            {
                return HttpContext.Session.Get<AdminUserInfo>("UserData");
            }
        }

        /// <summary>
        ///当前用户 是否是系统管理员
        /// </summary>
        /// <returns></returns>
        public bool IsSysRole
        {
            get
            {
                return UserData.RoleId.ToString().Equals(SysRoleId);
            }
        }

        /// <summary>
        ///系统管理员角色id
        /// </summary>
        /// <returns></returns>
        public static string SysRoleId
        {
            get
            {
                return ConfigurationManager.GetJsonValue("SysRoleId");
            }
        }

        #region  公共方法
        /// <summary>
        /// 获取地址
        /// </summary>
        /// <returns></returns>
        public string RouteUrl()
        {
            return " var BaseUrl = '/" + RouteData.Values["controller"].ToString() + "/';";
        }

        /// <summary>
        /// 操作按键列表  包含搜索
        /// </summary>
        /// <returns></returns>
        public string toolbar()
        {
            return toolbar(1);
        }

        /// <summary>
        /// 操作按键列表
        /// </summary>
        /// <param name="Ftype">1为 包含搜索，2 为不包含搜索</param>
        /// <returns></returns>
        public string toolbar(int Ftype)
        {
            string controller = RouteData.Values["controller"].ToString();
            string tool = " var toolbars =[";
            string search = "";
            int cout = 0;//统计
            Manu ManuItem = UserData.ListManusD[controller.ToLower()];
            if (ManuItem != null)//
            {
                //3.0以上版本

                foreach (var item in ManuItem.havebuttonsD)
                {
                    if (Ftype == 2 && item.Key.Equals("Search"))//搜索 不用添加进来
                    {

                    }
                    else
                    {
                        tool += "{";
                        tool += string.Format("id: '{0}',", item.Value.valuename);
                        tool += string.Format("text: '{0}',", item.Value.buttonname);
                        tool += string.Format("iconCls: '{0}',", item.Value.icon);
                        tool += "handler: function () { " + item.Value.functionname + "(); }}";
                        tool += ",'-',";
                        cout += 1;
                    }
                }
                if (cout > 0)
                {
                    tool = tool.Substring(0, tool.Length - 5);
                }
            }
            tool += "];";
            return tool + search;
        }

        public static string GetSql(string sqlSet)
        {
            string[] data = sqlSet.Split('█');
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(sqlSet))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    int index = data[i].IndexOf(":");
                    var nameData = data[i].Substring(0, index);

                    string[] name = nameData.Split('_');
                    string value = FilterTools.FilterSpecial(data[i].Substring(index + 1));
                    sql += " and " + GetOP(name[0], name[1], value);

                }
            }
            return sql;
        }


        static string GetOP(string name, string op, string values)
        {
            #region  多字段 模糊查询  如： OwnerName|OwnerCode|BuildingCode|HouseCode_like
            string[] names = name.Split('|');
            if (names.Length > 1)
            {
                string sql = "(";
                for (int i = 0; i < names.Length; i++)
                {
                    if (op.Equals("like"))
                    {
                        sql += names[i] + " like N'%" + values + "%' ";

                        if (i != names.Length - 1)
                        {
                            sql += " or ";
                        }
                    }
                }
                sql += ")";
                return sql;
            }
            #endregion


            switch (op)
            {
                case "like"://all

                    return name + " like N'%" + values + "%' ";
                case "like1":// 前固定

                    return name + " like N'" + values + "%' ";
                case "like2"://后固定

                    return name + " like N'%" + values + "' ";

                case "eq":

                    return name + " = '" + values + "' ";


                case "lt":

                    return name + " < '" + values + "' ";


                case "le":

                    return name + " <= '" + values + "' ";

                case "gt":

                    return name + " > '" + values + "' ";


                case "ge":

                    return name + " >= '" + values + "' ";


                case "ne":

                    return name + " != '" + values + "' ";
                default:
                    return "";
            }
        }
        #endregion


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool IsNull = false;
            IsHaveAtion = false;
            string controller = RouteData.Values["controller"].ToString();
            if(controller == "AjaxHandler")
                base.OnActionExecuting(context);

            else
            {
                string action = RouteData.Values["action"].ToString();
                if (UserData == null)
                {
                    //不用登录的页面一定要写在这里，不然会死循环
                    string[] IgnoreP = { "login", "clientuserview", "dictionaryview" };
                    if (!IgnoreP.Contains(controller.ToLower()))
                    {
                        IsNull = true;
                    }
                }
                else
                {
                    //不用权限控制的页面一定要写在这里，不然会死循环
                    string[] IgnoreP = { "home", "login", "main" };
                    if (!IgnoreP.Contains(controller.ToLower()) && !UserData.ListManusD.ContainsKey(controller.ToLower()))//没有这个菜单
                    {
                        IsNull = true;
                    }
                    //是否包含菜单
                    else if (UserData.ListManusD.ContainsKey(controller.ToLower()))
                    {
                        Manu ManuItem = UserData.ListManusD[controller.ToLower()];
                        //菜单是否包含按钮
                        if (ManuItem.havebuttonsD.ContainsKey(action) || ManuItem.nobuttonD.ContainsKey(action))
                        {
                            if (!ManuItem.havebuttonsD.ContainsKey(action))//当前权限 是否有这个按钮
                            {
                                IsNull = true;//这个很变态,少了就不能取消操作
                                IsHaveAtion = true;
                            }
                        }
                    }
                    else
                    {

                    }
                }
                if (IsNull)//非法操作一律返回登录
                {
                    context.Result = RedirectToAction("Index", "Login");
                    //context.HttpContext.Response.WriteAsync("<script>location.href='Login'</script>");
                }
                else
                {
                    base.OnActionExecuting(context);
                }
            }       
        }

        /// <summary>
        /// 标注Ation是否取消
        /// </summary>
        public bool IsHaveAtion
        {
            get;
            set;
        }

        public override JsonResult Json(object data, JsonSerializerSettings serializerSettings)
        {
            return base.Json(data, serializerSettings);
        }
    }

   
}
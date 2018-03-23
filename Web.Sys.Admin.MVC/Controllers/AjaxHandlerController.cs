using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.Model.RMS;
using Web.Sys.BLL.RMS;
using System.Data;
using Core.PetaPocoServer;
using Web.Sys.BLL;
using Web.Sys.Model;

namespace Web.Sys.Admin.MVC.Controllers
{
    /// <summary>
    /// 公用Ajax处理类
    /// </summary>
    public class AjaxHandlerController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCompany()
        {
            List<rms_company> AllList = new List<rms_company>();
            string sql = " 1=1 ";
            if (!IsSysRole)//如果不是系统管理员 只返回自已的
            {
                sql = " id=" + UserData.companyid;
            }
            AllList =RMS_CompanyService.Instance.FetchW(sql);
            return Json(AllList);
        }

        /// <summary>
        /// 角色类型
        /// </summary>
        /// <param name="RoleTypes"></param>
        /// <returns></returns>
        public JsonResult GetList(string RoleTypes)
        {
            string wheresql = "roletypes=" + RoleTypes;
            wheresql += " and companyid=" + UserData.companyid + " ";
            DataTable tb =RMS_RoleService.Instance.sqlToDataTable(new Sql(" select id,projectname,rolename,remarks from v_role where " + wheresql));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            // var mql = RMS_RoleSet.ControlId.NotEqual("");
            dic.Add("rows", tb);
            dic.Add("total", tb.Rows.Count);

            return Json(dic);
        }

        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public JsonResult GetUser()
        {
            return Json(UserData);
        }

        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDepartment()
        {
            List<rms_department> AllList = new List<rms_department>();
            string sql = " companyid=" +UserData.companyid;//只返回自己公司的
            AllList =RMS_DepartmentService.Instance.FetchW(sql);
            return Json(AllList);
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <returns></returns>
        public JsonResult Getpoject()
        {
            List<rms_project> AllList = new List<rms_project>();
            string sql = " companyid=" + UserData.companyid;//只返回自己公司的
            AllList = RMS_ProjectService.Instance.FetchW(sql);
            return Json(AllList);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using e3net.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.BLL.RMS;
using Web.Sys.BLL;
using Web.Sys.Model;
using Core.PetaPocoServer;
using e3net.Common.Json;
using Newtonsoft.Json;
using e3net.Common;

namespace Web.Sys.Admin.MVC.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.Message = "用户管理";
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
        {          
            int pageIndex = string.IsNullOrEmpty(Request.Form["page"])? 1 : int.Parse(Request.Form["page"]);
            int pageSize = string.IsNullOrEmpty(Request.Form["rows"])?10 : int.Parse(Request.Form["rows"]);
            string Where = string.IsNullOrEmpty(Request.Form["sqlSet"])? "1=1" : GetSql(Request.Form["sqlSet"]);
            ////字段排序
            String sortField = Request.Form["sort"];
            String sortOrder = Request.Form["order"];
            PageClass pc = new PageClass();
            pc.sys_Fields = "*";
            //pc.sys_Key = "id";
            pc.sys_PageIndex = pageIndex;
            pc.sys_PageSize = pageSize;
            pc.sys_Table = "v_userrole";

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                pc.sys_Order = " order by " + " " + sortField + " " + sortOrder;
            }
            string DepartmentId = Request.Form["DepartmentId"];
            if (!string.IsNullOrEmpty(DepartmentId))
            {

                Where += " and departmentid='" + DepartmentId + "'";

            }
            //if (!IsSysRole)//如果不是开发人员 只返回自已的
            //{
            Where += " and companyid=" + UserData.companyid + " ";
            //}

            pc.sys_Where = Where;
            DataTable ds = RMS_UserService.Instance.sqlToDataTablePage(pc);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            //net core 不知道DataTable序列化
            dic.Add("rows",ModelConvertHelper<rms_user>.ConvertToModel(ds));
            dic.Add("total", pc.RCount);

            return Json(dic);
        }

        public JsonResult EditInfo(rms_user EidModle)
        {
            ReSultMode ReSultMode = new ReSultMode();
            long i = RMS_UserService.Instance.ZGetCount(" LoginName='" + EidModle.loginname.Trim() + "' and id!='" + EidModle.id + "'");
            if (i > 0)
            {
                ReSultMode.code = -13;
                ReSultMode.data = "";
                ReSultMode.msg = "用户名已存在";
                return Json(ReSultMode);
            }

            bool IsAdd = false;
            if (!(EidModle.id != null && !EidModle.id.ToString().Equals("00000000-0000-0000-0000-000000000000")))//id为空，是添加
            {
                IsAdd = true;
            }

            EidModle.modifytime = DateTime.Now;
            if (IsAdd)
            {
                if (string.IsNullOrEmpty(EidModle.password))
                {
                    ReSultMode.code = -13;
                    ReSultMode.data = "";
                    ReSultMode.msg = "密码为空";
                }
                else
                {
                    EidModle.id = Guid.NewGuid().ToString();
                    EidModle.createtime = DateTime.Now;                 
                    EidModle.companyid = UserData.companyid;//只添加自己公司
                    RMS_UserService.Instance.Insert(EidModle);
                    ReSultMode.code = 11;
                    ReSultMode.data = EidModle.id.ToString();
                    ReSultMode.msg = "添加成功";
                }
            }
            else
            {

                if (string.IsNullOrEmpty(EidModle.password))//为空，密码不改
                {
                    rms_user olde = RMS_UserService.Instance.SingleM(EidModle.id);
                    EidModle.password = olde.password;
                }
                List<string> nocolumns = new List<string>();
                nocolumns.Add("companyid");
                nocolumns.Add("createtime");
                if (RMS_UserService.Instance.UpdateNoIn(EidModle, nocolumns) > 0)
                {
                    ReSultMode.code = 11;
                    ReSultMode.data = "";
                    ReSultMode.msg = "修改成功";
                }
                else
                {
                    ReSultMode.code = -13;
                    ReSultMode.data = "";
                    ReSultMode.msg = "修改失败";
                }
            }
            return Json(ReSultMode);
        }

        public JsonResult GetInfo(string ID)
        {
            rms_user Rmodel = RMS_UserService.Instance.SingleM(ID);
            return Json(Rmodel);
        }


        public JsonResult DeleteInfo(string ID)
        {
            string[] idSet = ID.Split(new string[] { "," }, StringSplitOptions.None);
            int cout = 0;
            foreach (string ids in idSet)
            {
                string id = ids.Replace("'", "");
                cout += RMS_UserService.Instance.deteUser(id);
            }
            return Json("OK");
        }

        public JsonResult SetRole(string UserId, string RoleId)
        {
            rms_userrole Rmodel =RMS_UserRoleService.Instance.First(new Sql(" select * from rms_userrole where userid='" + UserId + "'"));
            if (Rmodel == null)
            {
                Rmodel = new rms_userrole();
                Rmodel.id = Guid.NewGuid().ToString();
                Rmodel.userid = UserId;
                Rmodel.roleid = RoleId;
                RMS_UserRoleService.Instance.Insert(Rmodel);
                return Json("ok");
            }
            else
            {
                Rmodel.roleid = RoleId;
                if (RMS_UserRoleService.Instance.Update(Rmodel) > 0)
                {
                    return Json("ok");
                }
                else
                {
                    return Json("Nok");
                }
            }

        }
    }
}
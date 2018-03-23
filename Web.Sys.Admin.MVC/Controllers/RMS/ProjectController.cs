using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using e3net.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.BLL.RMS;
using Web.Sys.BLL;
using Web.Sys.Model;
using Web.Sys.Model.RMS;

namespace Web.Sys.Admin.MVC.Controllers.RMS
{
    public class ProjectController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.RuteUrl = RouteUrl();
            ViewBag.toolbar = toolbar(2);
            return View();
        }

        [HttpPost]
        public JsonResult Search()
        {
            int pageIndex = string.IsNullOrEmpty(Request.Form["page"]) ? 1 : int.Parse(Request.Form["page"]);
            int pageSize = string.IsNullOrEmpty(Request.Form["rows"]) ? 10 : int.Parse(Request.Form["rows"]);
            string Where = string.IsNullOrEmpty(Request.Form["sqlSet"]) ? "1=1" : GetSql(Request.Form["sqlSet"]);
            ////字段排序
            String sortField = Request.Form["sort"];
            String sortOrder = Request.Form["order"];
            PageClass pc = new PageClass();
            pc.sys_Fields = "*";
            //pc.sys_Key = "id";
            pc.sys_PageIndex = pageIndex;
            pc.sys_PageSize = pageSize;
            pc.sys_Table = "v_rms_project";

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {

                pc.sys_Order = " order by " + " " + sortField + " " + sortOrder;
            }
            if (!IsSysRole)//如果不是开发人员 只返回自已的
            {
                Where += " and companyid=" + UserData.companyid + " ";
            }
            pc.sys_Where = Where;
            //if (!UserData.UserInfo.RoleId.ToString().Equals("fb38f312-0078-4f44-9cda-1183c8042db8"))//不是系统管理员，限制一个医院
            //{
            //    pc.sys_Where += " and YH_HospitalId='" + UserData.UserInfo.YH_HospitalId + "'";
            //}

            DataTable ds =RMS_ProjectService.Instance.sqlToDataTablePage(pc);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("rows", ds);
            dic.Add("total", pc.RCount);
            return Json(dic);
        }

        public JsonResult EditInfo(rms_project EidModle)
        {
            ReSultMode ReSultMode = new ReSultMode();

            bool IsAdd = false;
            EidModle.modifytime = DateTime.Now;
            if (EidModle.id == 0)//id为空，是添加
            {
                IsAdd = true;
                EidModle.createtime = DateTime.Now;
                EidModle.isvalid = true;
                EidModle.isdeleted = false;
            }

            if (IsAdd)
            {
                try
                {
                    object i = RMS_ProjectService.Instance.Insert(EidModle);
                    ReSultMode.code = 11;
                    ReSultMode.data = i.ToString();
                    ReSultMode.msg = "添加成功";
                }
                catch (Exception e)
                {

                    ReSultMode.code = -11;
                    ReSultMode.data = e.ToString();
                    ReSultMode.msg = "添加失败";
                }
            }
            else
            {
                List<string> nocolumns = new List<string>();
                nocolumns.Add("createtime");
                nocolumns.Add("keys");
                if (RMS_ProjectService.Instance.UpdateNoIn(EidModle, nocolumns) > 0)
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

            rms_project Rmodel = RMS_ProjectService.Instance.SingleM(ID);
            Rmodel.keys = "";//不显示到ui层
            //  groupsBiz.Add(rol);
            return Json(Rmodel);
        }


        public JsonResult Del(string IDSet)
        {
            ReSultMode ReSultMode = new ReSultMode();
            if (!IsSysRole)//如果不是系统开发员
            {
                ReSultMode.code = -13;
                ReSultMode.data = "0";
                ReSultMode.msg = "删除失败！你不是系统开发员";
                return Json(ReSultMode);
            }
            string[] idSet = IDSet.Split(new string[] { "," }, StringSplitOptions.None);
            int cout = 0;
            foreach (string ids in idSet)
            {
                int id = int.Parse(ids.Replace("'", ""));
                cout += RMS_ProjectService.Instance.deleteProject(id);
            }
            if (cout > 0)
            {
                ReSultMode.code = 11;
                ReSultMode.data = cout.ToString();
                ReSultMode.msg = "成功删除" + cout + "条数据！";
                return Json(ReSultMode);
            }
            else
            {
                ReSultMode.code = -13;
                ReSultMode.data = "0";
                ReSultMode.msg = "删除失败！";
                return Json(ReSultMode);
            }
        }
    }
}
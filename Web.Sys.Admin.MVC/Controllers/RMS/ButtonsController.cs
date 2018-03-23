using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using e3net.Common;
using e3net.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.BLL;
using Web.Sys.BLL.RMS;
using Web.Sys.Model.RMS;


namespace Web.Sys.Admin.MVC.Controllers
{
    public class ButtonsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetList()
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
            pc.sys_Table = "rms_buttons";
            pc.sys_Where = Where;
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                pc.sys_Order = " order by " + " " + sortField + " " + sortOrder;
            }
            DataTable ds = RMS_ButtonsService.Instance.sqlToDataTablePage(pc);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("rows", ModelConvertHelper<rms_buttons>.ConvertToModel(ds));
            dic.Add("total", pc.RCount);

            return Json(dic);
        }

        public JsonResult EditInfo(rms_buttons RMS_ButtonsModle)
        {

            bool IsAdd = false;
            if (RMS_ButtonsModle.id == 0)//id为空，是添加
            {
                IsAdd = true;
            }
            if (IsAdd)
            {
                //RMS_ButtonsModle.id = Guid.NewGuid().ToString();
                RMS_ButtonsModle.createtime = DateTime.Now;
                RMS_ButtonsModle.modifytime = DateTime.Now;
                //rol.RoleDescription = RMS_ButtonsModle.RoleDescription;
                //rol.RoleOrder = RMS_ButtonsModle.RoleOrder;

                RMS_ButtonsService.Instance.Insert(RMS_ButtonsModle);
                return Json("ok");
            }
            else
            {
                if (RMS_ButtonsService.Instance.Update(RMS_ButtonsModle) > 0)
                {
                    return Json("ok");
                }
                else
                {
                    return Json("Nok");
                }
            }
        }
        public JsonResult GetInfo(string ID)
        {
            rms_buttons Rmodel = RMS_ButtonsService.Instance.SingleM(ID);
            //  groupsBiz.Add(rol);
            return Json(Rmodel);
        }


        public JsonResult DeleteInfo(string ID)
        {
            if (!IsSysRole)//如果不是系统开发员
            {
                return Json("Nok");
            }
            int f = RMS_ButtonsService.Instance.DeleteM(ID);
            return Json("OK");
        }
    }
}
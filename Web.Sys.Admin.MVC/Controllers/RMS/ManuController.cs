using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e3net.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.Model.RMS;
using Web.Sys.BLL.RMS;
using Core.PetaPocoServer;
using Web.Sys.Model.Common;

namespace Web.Sys.Admin.MVC.Controllers.RMS
{
    public class ManuController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }


        public JsonResult EditInfo(rms_menus Mode)
        {
            ReSultMode res = new ReSultMode();

            rms_menus old =RMS_MenusService.Instance.SingleW(" url='" + Mode.url + "' and projectid=" + Mode.projectid);
            if (old != null && Mode.id != old.id)
            {
                res.code = -11;
                res.msg = "地址 url已存在";
                return Json(res);
            }

            bool IsAdd = false;
            if (Mode.id == 0)//id为空，是添加
            {
                IsAdd = true;
            }
            if (IsAdd)
            {
                //Mode.id = Guid.NewGuid().ToString(""); 
                Mode.createtime = DateTime.Now;
                Mode.modifytime = DateTime.Now;
                Mode.companyid = UserData.companyid;
                Mode.isenable = true;
                Mode.isshow = true;
                RMS_MenusService.Instance.Insert(Mode);

                res.code = 11;
                res.msg = "添加成功";

                return Json(res);
            }
            else
            {
                Mode.modifytime = DateTime.Now;
                List<string> nocolumns = new List<string>();
                nocolumns.Add("companyid");
                nocolumns.Add("createtime");
                if (RMS_MenusService.Instance.UpdateNoIn(Mode, nocolumns) > 0)
                {

                    res.code = 11;
                    res.msg = "修改成功";

                    return Json(res);
                }
                else
                {
                    res.code = -11;
                    res.msg = "修改失败";
                    return Json(res);
                }
            }
        }
        public JsonResult GetInfo(string ID)
        {
            rms_menus Rmodel = RMS_MenusService.Instance.SingleM(ID);
            return Json(Rmodel);
        }

        public JsonResult GetOneOut(string ManuId)//获取菜单未添加的按钮
        {
            var query =RMS_ButtonsService.Instance.Query((" select * from rms_buttons where id not in( select buttonid from rms_menubuttons where manuid='" + ManuId + "') order by orderno asc"));
            List<rms_buttons> Rmodel = query.ToList();
            return Json(Rmodel);
        }
        public JsonResult GetOneIn(string ManuId)//获取菜单已经添加的按钮
        {
            Sql sql = new Sql(" select * from v_menubuttons where manuid='" + ManuId + "' order by orderno asc");
            List<v_menubuttons> Rmodel =V_MenuButtonsService.Instance.Fetch(sql);//已经添加的按钮
            return Json(Rmodel);
        }

        public JsonResult GetJson(string projectid)
        {
            string sqlstr = " select * from v_rms_menus";
            //if (!IsSysRole)//如果不是开发人员 只返回自已的
            //{
            sqlstr += " where companyid=" + UserData.companyid + " ";
            //}
            if (!string.IsNullOrEmpty(projectid))
            {
                sqlstr += " and projectid='" + projectid + "'";
            }
            List<v_rms_menus> listAll =V_RMS_MenusService.Instance.Fetch<v_rms_menus>(sqlstr).AsQueryable().ToList();
            List<TreeMenus> listTree =RMS_MenusService.Instance.GetTreeManus(listAll);
            return Json(listTree);
        }

        public JsonResult DeleteInfo(string ID)
        {
            string[] idSet = ID.Split(new string[] { "," }, StringSplitOptions.None);
            int cout = 0;
            foreach (string ids in idSet)
            {
                int id = int.Parse(ids.Replace("'", ""));
                cout +=RMS_MenusService.Instance.deletemenu(id);
            }
            if (cout > 0)
            {
                return Json("OK");
            }
            else
            {

                return Json("Nok");
            }
        }
        /// <summary>
        /// //添加单按钮
        /// </summary>
        /// <param name="btnId"></param>
        /// <param name="ManuId"></param>
        /// <returns></returns>
        public JsonResult AddManuBtn(string BtnId, string ManuId, string OrderNo)
        {
            string sql = " select * from rms_menubuttons where buttonid='" + BtnId + "' and manuid='" + ManuId + "'";
            rms_menubuttons item =RMS_MenuButtonsService.Instance.First(sql);
            if (item != null)
            {
                item.orderno = int.Parse(OrderNo);
                //  spmodel.GroupId = GroupId;
                if (RMS_MenuButtonsService.Instance.Update(item) > 0)
                {
                    return Json("OK");
                }
            }
            item = new rms_menubuttons();
            //item.id = Guid.NewGuid().ToString();
            item.buttonid = int.Parse(BtnId);
            item.manuid = int.Parse(ManuId);
            item.orderno = int.Parse(OrderNo);
            RMS_MenuButtonsService.Instance.Insert(item);
            return Json("OK");
        }

        /// <summary>
        /// //删菜单按钮
        /// </summary>
        /// <param name="btnId"></param>
        /// <param name="ManuId"></param>
        /// <returns></returns>
        public JsonResult DelManuBtn(string IdSet, string ManuId)
        {
            if (!IsSysRole)//如果不是系统开发员
            {
                return Json("Nok");
            }
            //List<string> ids = IdSet.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Sql sql = new Sql(" where buttonid in (" + IdSet + ") and manuid='" + ManuId + "'");
            int f = RMS_MenuButtonsService.Instance.Delete(sql);
            return Json("OK");
        }
    }
}
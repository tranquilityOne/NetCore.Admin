using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using e3net.Common.Entity;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.BLL.RMS;
using Web.Sys.BLL;
using Web.Sys.Model.RMS;
using Web.Sys.Model;
using e3net.Common;

namespace Web.Sys.Admin.MVC.Controllers.RMS
{
    public class RoleController : BaseController
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
            pc.sys_Table = "v_role";

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                pc.sys_Order = " order by " + " " + sortField + " " + sortOrder;
            }
            if (!IsSysRole)//不是系统管理员，不请允许显示系统管理员
            {
                Where += " and id!='" + SysRoleId + "'";
            }
            //if (!IsSysRole)//如果不是开发人员 只返回自已的
            //{
            Where += " and companyid=" + UserData.companyid + " ";
            //}
            pc.sys_Where = Where;
            DataTable ds =RMS_RoleService.Instance.sqlToDataTablePage(pc);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("rows",ModelConvertHelper<rms_role>.ConvertToModel(ds));
            dic.Add("total", pc.RCount);
            return Json(dic);
        }
        public JsonResult EditInfo(rms_role RMS_RoleModle)
        {
            ReSultMode ReSultMode = new ReSultMode();
            bool IsAdd = false;

            RMS_RoleModle.modifytime = DateTime.Now;
            if (!(RMS_RoleModle.id != null && !RMS_RoleModle.id.ToString().Equals("00000000-0000-0000-0000-000000000000")))//id为空，是添加
            {
                IsAdd = true;
                RMS_RoleModle.createtime = DateTime.Now;
                RMS_RoleModle.id = Guid.NewGuid().ToString();
            }
            if (IsAdd)
            {
                RMS_RoleModle.modifyby = "1";
                RMS_RoleModle.createby = "1";
                RMS_RoleModle.companyid = UserData.companyid;//只添加自己公司
                try
                {
                    RMS_RoleService.Instance.Insert(RMS_RoleModle);
                    ReSultMode.code = 11;
                    ReSultMode.data = RMS_RoleModle.id.ToString();
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
                nocolumns.Add("companyid");
                nocolumns.Add("createtime");
                nocolumns.Add("jurisdiction");//权限不能在这里更改
                if (RMS_RoleService.Instance.UpdateNoIn(RMS_RoleModle, nocolumns) > 0)
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
            rms_role Rmodel = RMS_RoleService.Instance.SingleM(ID);
            Rmodel.jurisdiction = "";//不显示到ui层
            //  groupsBiz.Add(rol);
            return Json(Rmodel);
        }


        public JsonResult DeleteInfo(string ID)
        {
            string[] idSet = ID.Split(new string[] { "," }, StringSplitOptions.None);
            int cout = 0;
            foreach (string id in idSet)
            {
                cout += RMS_RoleService.Instance.deleteRole(id);
            }
            return Json("OK");

        }
        /// <summary>
        /// 获取某个角色的权限 数据不要乱改啊！！
        /// </summary>
        /// <param name="Id">角色的Id</param>
        /// <returns></returns>
        public string GetManeOP(string Id)
        {
            rms_role Rmodel = RMS_RoleService.Instance.SingleM(Id);//当前角色 
            string menus = " [\n";
            var sqlManu = " 1=1 ";
            sqlManu += " and companyid=" + UserData.companyid + " ";//只显示 自己公司的
            sqlManu += " and projectid=" + Rmodel.projectid + " ";//只显示同一个项目
            if (!IsSysRole)//不是系统管理员的角色，只能返回自己的菜单
            {
                sqlManu += " and id in( select manuid from rms_rolemanus where roleid='" + UserData.RoleId + "')";

            }

            List<rms_menus> list =RMS_MenusService.Instance.FetchW(sqlManu);//菜单集
            List<rms_buttons> listControlButtons =RMS_ButtonsService.Instance.FetchW(" 1=1 ");//所有的按钮
            List<rms_rolemanus> listRoleColumns =RMS_RoleManusService.Instance.FetchW(" roleid='" + Id + "'");//这个角色已经添加的菜单

            List<v_rolemanubuttons> listRoleMenuButtons = V_RoleManuButtonsService.Instance.FetchW(" roleid='" + Id + "'");//这个角色已经添加的角色按钮
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Find(p => p.id == list[i].parentmanuid) == null)//此项没有父级
                {

                    menus += "{  \"MenuId\":\"" + list[i].id + "\",";
                    menus += string.Format("  \"Name\":\"{0}\",", list[i].manuname);
                    menus += string.Format("  \"iconCls\":\"{0}\",", list[i].icon);
                    string ControlId_Browse = "0";
                    rms_rolemanus rcItem = listRoleColumns.Find(p => p.manuid.Equals(list[i].id));

                    if (rcItem != null)
                    {
                        ControlId_Browse = "1";//如果存在

                    }
                    menus += string.Format("  \"ControlId_Browse\":\"{0}\",", ControlId_Browse);
                    foreach (rms_buttons dd in listControlButtons)//添加列的数据
                    {

                        string Ishave = "0";
                        v_rolemanubuttons rmbItem = listRoleMenuButtons.Find(p => p.id.Equals(dd.id) && p.manuid.Equals(list[i].id));

                        if (rmbItem != null)
                        {
                            Ishave = "1";//如果角色有此按钮存在

                        }
                        menus += string.Format("\"ControlId_{0}\":\"{1}\",", dd.id, Ishave);

                    }
                    menus += GetSonTreeManu(list, list[i], listControlButtons, listRoleColumns, listRoleMenuButtons);//添加children
                    menus += "},";
                }
            }
            menus = menus.Substring(0, menus.Length - 1);
            menus = menus + "]";

            return menus;
        }
        private string GetSonTreeManu(List<rms_menus> listAll, rms_menus SonItem, List<rms_buttons> listControlButtons, List<rms_rolemanus> listRoleColumns, List<v_rolemanubuttons> listRoleMenuButtons)
        {
            string menus = "\"children\":[";
            List<rms_menus> list = listAll.FindAll(p => p.parentmanuid.Equals(SonItem.id));
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {


                    menus += "{  \"MenuId\":\"" + list[i].id + "\",";
                    menus += string.Format("  \"Name\":\"{0}\",", list[i].manuname);
                    menus += string.Format("  \"iconCls\":\"{0}\",", list[i].icon);
                    string ControlId_Browse = "0";
                    rms_rolemanus rcItem = listRoleColumns.Find(p => p.manuid.Equals(list[i].id));

                    if (rcItem != null)
                    {
                        ControlId_Browse = "1";//如果存在

                    }
                    menus += string.Format("  \"ControlId_Browse\":\"{0}\",", ControlId_Browse);
                    foreach (rms_buttons dd in listControlButtons)//添加列的数据
                    {

                        string Ishave = "0";

                        v_rolemanubuttons rmbItem = listRoleMenuButtons.Find(p => p.id.Equals(dd.id) && p.manuid.Equals(list[i].id));

                        if (rmbItem != null)
                        {
                            Ishave = "1";//如果角色有此按钮存在

                        }

                        menus += string.Format("\"ControlId_{0}\":\"{1}\",", dd.id, Ishave);

                    }
                    menus += GetSonTreeManu(listAll, list[i], listControlButtons, listRoleColumns, listRoleMenuButtons);//添加children
                    menus += "},";

                }
                menus = menus.Substring(0, menus.Length - 1);
            }
            menus = menus + "]";
            return menus;
        }
        /// <summary>
        /// 获取列
        /// </summary>
        /// <returns></returns>
        public string GetBtnColumn()
        {

            string menus = " [\n";
            List<rms_buttons> list =RMS_ButtonsService.Instance.FetchW(" 1=1 ");
            if (list != null)
            {
                menus += "{  ";

                menus += "title:\"名称\",field:\"Name\", width: 100";
                menus += "},";
                menus += "{  ";

                menus += "title:\"浏览\",field:\"ControlId_Browse\", width: 30,editor:{type:'checkbox',options:{on:'1',off:'0'}}, formatter: formatCheck";
                menus += "},";

                foreach (rms_buttons item in list)
                {
                    menus += "{  ";

                    menus += "title:\"" + item.buttonname + "\",field:\"ControlId_" + item.id + "\", width: 30,editor:{type:'checkbox',options:{on:'1',off:'0'}}, formatter: formatCheck";
                    menus += "},";
                }

            }

            menus = menus.Substring(0, menus.Length - 1);
            menus = menus + "]";

            return menus;

        }

        /// <summary>
        /// 获取每个菜单有的按钮
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMenuButtonsData()
        {
            string sql = " 1=1 ";
            if (!IsSysRole)//不是系统管理员的角色，只能返回自己的
            {
                sql = " id  in( select menubuttonsid from rms_rolemanubuttons where roleid='" + UserData.RoleId + "')";

            }
            List<rms_menubuttons> listMenuButtons =RMS_MenuButtonsService.Instance.FetchW(sql);//菜单的按钮
            return Json(listMenuButtons);
        }

        public string SaveRoleOP(string RoleManus, string RoleManuButtons, string RoleId)
        {

            RMS_RoleManusService.Instance.GetDatabase().BeginTransaction();//事务开始
            RMS_RoleManuButtonsService.Instance.GetDatabase().BeginTransaction();//事务开始
            RMS_RoleService.Instance.GetDatabase().BeginTransaction();//事务开始
            try
            {
                List<string> manu = RoleManus.Split('_').ToList();
                int res = 0;
                int f = RMS_RoleManusService.Instance.DeleteW(" roleid='" + RoleId + "'");
                for (int i = 0; i < manu.Count; i++)
                {

                    if (!string.IsNullOrEmpty(manu[i]))
                    {
                        rms_rolemanus item = new rms_rolemanus();
                        //item.id = Guid.NewGuid().ToString();
                        item.roleid = RoleId;
                        item.manuid = int.Parse(manu[i]);
                        RMS_RoleManusService.Instance.Insert(item);
                        res++;
                    }
                }
                List<string> ManuButtons = RoleManuButtons.Split('_').ToList();
                int ff = RMS_RoleManuButtonsService.Instance.DeleteW(" roleid='" + RoleId + "'");
                for (int i = 0; i < ManuButtons.Count; i++)
                {

                    rms_rolemanubuttons item = new rms_rolemanubuttons();
                    //item.id = Guid.NewGuid().ToString();
                    string[] data = ManuButtons[i].Split(':');
                    if (!string.IsNullOrEmpty(data[0]) && !string.IsNullOrEmpty(data[1]))
                    {
                        item.roleid = RoleId;

                        string sql = " manuid=" + data[0] + " and buttonid=" + data[1] + "";
                        rms_menubuttons bItem = RMS_MenuButtonsService.Instance.SingleW(sql);
                        if (bItem != null)
                        {
                            item.menubuttonsid = bItem.id.ToString();
                        }

                        RMS_RoleManuButtonsService.Instance.Insert(item);
                        res++;
                    }
                }

                bool isSucces = true;//操作是否成功
                //if (res == manu.Count + ManuButtons.Count)
                //{

                //    #region  生成权限 json
                //    bool ies = RDBiz.SetJurisdiction(RoleId);//保存 权限json
                //    if (ies)
                //    {
                //        isSucces = true;
                //    }
                //    else
                //    {

                //        isSucces = false;
                //    }


                //    #endregion
                //}
                //else
                //{
                //    isSucces = false;
                //}

                if (isSucces)//是否成功
                {
                    RMS_RoleManusService.Instance.GetDatabase().CompleteTransaction();//事务完成
                    RMS_RoleManuButtonsService.Instance.GetDatabase().CompleteTransaction();//事务完成
                    RMS_RoleService.Instance.GetDatabase().CompleteTransaction();//事务完成

                    bool ies = RMS_RoleService.Instance.SetJurisdiction(RoleId);//保存 权限json
                    return "ok";

                }
                else
                {
                    RMS_RoleManusService.Instance.GetDatabase().AbortTransaction();//事务回滚
                    RMS_RoleManuButtonsService.Instance.GetDatabase().AbortTransaction();//事务回滚
                    RMS_RoleService.Instance.GetDatabase().AbortTransaction();//事务回滚
                    return "Nok";

                }
            }
            catch (Exception ex)
            {
                RMS_RoleManusService.Instance.GetDatabase().AbortTransaction();//事务回滚
                RMS_RoleManuButtonsService.Instance.GetDatabase().AbortTransaction();//事务回滚
                RMS_RoleService.Instance.GetDatabase().AbortTransaction();//事务回滚
                return "Nok";
            }
        }



        public JsonResult Refreshpermissions(string roleid)
        {
            ReSultMode ReSultMode = new ReSultMode();
            bool issusess = RMS_RoleService.Instance.SetJurisdiction(roleid);
            if (issusess)
            {
                ReSultMode.code = 11;
                ReSultMode.msg = "刷新权限成功";
            }
            else
            {
                ReSultMode.code = -11;
                ReSultMode.msg = "刷新权限失败";

            }
            return Json(ReSultMode);
        }

    }
}
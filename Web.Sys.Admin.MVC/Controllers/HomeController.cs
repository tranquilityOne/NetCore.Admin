using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Sys.Admin.MVC.Models;
using Web.Sys.Model;
using Web.Sys.Model.Common;
using Web.Sys.BLL;

namespace Web.Sys.Admin.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.userName = UserData.UserName;
            ViewBag.companyName = UserData.companyName;
            return View();
        }

        #region 递归树形菜单加载
        public JsonResult GetMenu()
        {
            IEnumerable<KeyValuePair<string, Manu>> ListManusD = UserData.ListManusD;//所有的子集 不包含自已
            List<m_v_rolemanus> listTwoSet = new List<m_v_rolemanus>();//父级
            List<m_v_rolemanus> listTwoUrl = new List<m_v_rolemanus>();//二级的连接

            foreach (var item in ListManusD)
            {
                if (item.Value.manuinfo.typeid == 2)
                {
                    listTwoUrl.Add(item.Value.manuinfo);
                }
                else
                {
                    listTwoSet.Add(item.Value.manuinfo);
                }
            }
            List<TreeMenus> listTree = GetTreeMenus(listTwoSet, listTwoUrl);
            return Json(listTree);
        }

        private List<TreeMenus> GetTreeMenus(List<m_v_rolemanus> listTwoSet, List<m_v_rolemanus> listTwoUrl)
        {
            List<TreeMenus> resultList = new List<TreeMenus>();//总菜单
            foreach (var item in listTwoSet)//所有的二级集合
            {
                List<m_v_rolemanus> listson = new List<m_v_rolemanus>();//这个二级的子集
                for (int i = 0; i < listTwoUrl.Count; i++)
                {
                    if (listTwoUrl[i].parentmanuid.Equals(item.manuid))
                    {
                        listson.Add(listTwoUrl[i]);
                        listTwoUrl.Remove(listTwoUrl[i]);
                        i--;
                    }
                }
                resultList.Add(GetMenus(item, listson));
            }
            return resultList;
        }


        /// <summary>
        /// 父亲 和子集
        /// </summary>
        /// <param name="item"></param>
        /// <param name="listson"></param>
        /// <returns></returns>
        private TreeMenus GetMenus(m_v_rolemanus item, List<m_v_rolemanus> listson)
        {
            List<TreeMenus> resultList = new List<TreeMenus>();
            int allcout = listson.Count;
            for (int i = 0; i < allcout; i++)
            {
                TreeMenus resultItem = new TreeMenus();
                resultItem.Id = listson[i].manuid;
                resultItem.ParentManuId = listson[i].parentmanuid;
                resultItem.OrderNo = listson[i].orderno;
                resultItem.Name = listson[i].manuname;
                resultItem.iconCls = listson[i].icon;
                resultItem.MIcon = listson[i].micon;
                resultItem.URL = listson[i].url;
                resultList.Add(resultItem);
            }
            TreeMenus ItemN = new TreeMenus();
            ItemN.Id = item.manuid;
            ItemN.ParentManuId = item.parentmanuid;
            ItemN.OrderNo = item.orderno;
            ItemN.Name = item.manuname;
            ItemN.iconCls = item.icon;
            ItemN.MIcon = item.micon;
            ItemN.URL = item.url;
            ItemN.children = resultList;
            return ItemN;
        }
        #endregion

        /// <summary>
        /// 获取菜单头部视图,不同项目不同
        /// </summary>
        /// <returns></returns>
        public ActionResult WatchTop()
        {
            #region 返回头部菜单
            List<m_v_rolemanus> list = new List<m_v_rolemanus>();
            foreach (var item in UserData.ListManusD)
            {
                if (item.Value.manuinfo.typeid == 0)
                {
                    list.Add(item.Value.manuinfo);
                }
            }
            ViewBag.MenuData = list;
            #endregion

            ViewBag.UserName = UserData.UserName;
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult loginOut()
        {
            UserData = null;
            return Json("OK");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePwd(string NewPwd, string OldPwd)
        {
            if (!OldPwd.Trim().Equals(UserData.Password))
            {
                return Json("旧密码不成确");
            }
            else
            {
                rms_user item = RMS_UserService.Instance.SingleM(UserData.Id);
                item.password = NewPwd;
                if (RMS_UserService.Instance.Update(item) > 0)
                {
                    UserData.Password = NewPwd;
                    return Json("密码修改成功");
                }
                else
                {
                    return Json("密码修改失败");
                }
            }
        }

        /// <summary>
        /// 修改当前公司
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeCompany(string companyid, string companname)
        {
            UserData.companyid = int.Parse(companyid);
            UserData.companyName = companname;
            return Json("当前公司：" + companname);

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

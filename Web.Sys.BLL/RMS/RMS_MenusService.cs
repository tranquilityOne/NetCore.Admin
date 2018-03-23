using Core.PetaPocoServer;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Sys.Model.Common;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_MenusService : BaseDAL<rms_menus>
    {
        #region 单例实现
        private static RMS_MenusService singleInstance;

        private static object obj = new object();

        private RMS_MenusService() { }

        public static RMS_MenusService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_MenusService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deletemenu(int id)
        {
            GetDatabase().BeginTransaction();//事务开始
            try
            {
                string sqlstr = " delete from  rms_rolemanubuttons where menubuttonsid in(select id from  rms_menubuttons where manuid=" + id + ")";//删除角色菜单按钮
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_menubuttons where manuid=" + id;//删除菜单按钮
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_rolemanus where manuid=" + id;//删除菜单按钮
                Execute(new Sql(sqlstr));
                int res = DeleteM(id);
                GetDatabase().CompleteTransaction();//事务完成
                return res;
            }
            catch (Exception ex)
            {
                GetDatabase().AbortTransaction();//事务回滚
                return 0;
            }

        }
        /// <summary>
        /// 转化数型菜单
        /// </summary>
        /// <param name="mql"></param>
        /// <returns></returns>
        public List<TreeMenus> GetTreeManus(List<v_rms_menus> list)
        {
            List<TreeMenus> resultList = new List<TreeMenus>();
            List<v_rms_menus> listfather = list.FindAll(p => p.parentmanuid == 0);//父项
            listfather = listfather.OrderBy(i => i.orderno).ToList();
            int fatherCout = listfather.Count;
            for (int i = 0; i < fatherCout; i++)
            {
                TreeMenus resultItem = new TreeMenus();
                resultItem.Id = listfather[i].id;
                resultItem.ParentManuId = listfather[i].parentmanuid;
                resultItem.OrderNo = listfather[i].orderno;
                resultItem.Name = listfather[i].manuname;
                resultItem.iconCls = listfather[i].icon;
                resultItem.MIcon = listfather[i].micon;
                resultItem.URL = listfather[i].url;
                resultItem.valuename = listfather[i].valuename;
                resultItem.typeid = listfather[i].typeid;
                resultItem.Remarks = listfather[i].remarks;
                resultItem.projectname = list[i].projectname;
                List<TreeMenus> Son = GetTreeManus(list, listfather[i]);
                resultItem.children = Son;
                resultList.Add(resultItem);
            }
            return resultList;
        }
        public List<TreeMenus> GetTreeManus(List<v_rms_menus> listAll, v_rms_menus item)
        {
            List<TreeMenus> resultList = new List<TreeMenus>();
            List<v_rms_menus> list = listAll.FindAll(p => p.parentmanuid == item.id).OrderBy(i => i.orderno).ToList();
            for (int i = 0; i < list.Count; i++)
            {

                TreeMenus resultItem = new TreeMenus();
                resultItem.Id = list[i].id;
                resultItem.ParentManuId = list[i].parentmanuid;
                resultItem.OrderNo = list[i].orderno;
                resultItem.Name = list[i].manuname;
                resultItem.iconCls = list[i].icon;
                resultItem.MIcon = list[i].micon;
                resultItem.URL = list[i].url;
                resultItem.valuename = list[i].valuename;
                resultItem.typeid = list[i].typeid;
                resultItem.Remarks = list[i].remarks;
                resultItem.projectname = list[i].projectname;
                List<TreeMenus> Son = GetTreeManus(listAll, list[i]);
                resultItem.children = Son;
                resultList.Add(resultItem);
            }
            return resultList;
        }

    }
}

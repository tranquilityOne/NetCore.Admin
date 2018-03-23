using System;
using System.Collections.Generic;
using System.Linq;
using e3net.Common.Json;
using Web.Sys.Model;
using Core.PetaPocoServer;

namespace Web.Sys.BLL
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_RoleService : BaseDAL<rms_role>
    {
        #region 单例实现
        private static RMS_RoleService singleInstance;

        private static object obj = new object();

        private RMS_RoleService()
        {

        }

        public static RMS_RoleService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_RoleService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion

        /// <summary>
        /// 删除角色
        /// </summary>
        public int deleteRole(string id)
        {
            GetDatabase().BeginTransaction();//事务开始
            try
            {
                string sqlstr = " delete from  rms_rolemanus where roleid='" + id + "'";//删除角色菜单
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_rolemanubuttons where roleid='" + id + "'";//删除角色按钮
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_userrole where roleid='" + id + "'";//删除用户角色
                Execute(new Sql(sqlstr));
                int res = DeleteM(id);//删除角色

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
        /// 从数据库视图结构根据角色 获取拥有的菜单 按钮等 ()
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public Dictionary<string, Manu> GetRoleListManusD(string RoleId)
        {
            try
            {
                List<m_v_rolemanus> manus = FetchW<m_v_rolemanus>(" roleid='" + RoleId + "' order by orderno asc ");//所有的菜单
                List<m_v_rolemanubuttons> buttons = FetchW<m_v_rolemanubuttons>(" roleid='" + RoleId + "' order by orderno asc ");//角色拥有的菜单的所有按钮
                List<m_v_menubuttons> AllButtons = FetchW<m_v_menubuttons>("1=1");//所有菜单的所有按钮
                if (manus != null && manus.Count > 0)
                {
                    Dictionary<string, Manu> ListManusD = new Dictionary<string, Manu>();
                    foreach (m_v_rolemanus item in manus)
                    {

                        List<m_v_rolemanubuttons> Onehave = buttons.FindAll(p => p.manuid.Equals(item.manuid));//拥有的按钮
                        Dictionary<string, m_v_rolemanubuttons> havebuttonsD = Onehave.ToDictionary(p => p.functionname, p => p);//拥有的按钮

                        List<m_v_menubuttons> Allhave = AllButtons.FindAll(p => p.manuid.Equals(item.manuid));//这个菜单所有的按钮 
                        List<m_v_menubuttons> nohave = new List<m_v_menubuttons>();//角色没拥有的
                        for (int i = 0; i < Allhave.Count; i++)
                        {
                            if (!havebuttonsD.ContainsKey(Allhave[i].functionname))//如果没有
                            {
                                nohave.Add(Allhave[i]);
                            }
                        }
                        Dictionary<string, m_v_menubuttons> nolbuttonD = nohave.ToDictionary(p => p.functionname, p => p);//角色没有的 按钮 
                        Manu OneManu = new Manu();
                        OneManu.manuinfo = item;
                        OneManu.nobuttonD = nolbuttonD;//OrderBy(p => p.Value.orderno).ToDictionary(o => o.Key, p => p.Value);//排序;
                        OneManu.havebuttonsD = havebuttonsD.OrderBy(p => p.Value.orderno).ToDictionary(o => o.Key, p => p.Value); ;//排序;

                        ListManusD.Add(item.url.ToLower(), OneManu);
                    }
                    ListManusD.OrderBy(p => p.Value.manuinfo.orderno);
                    return ListManusD;
                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 更新权限，重新生成权限json，保存到角色里
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public bool SetJurisdiction(string RoleId)
        {
            bool isSucces = false;
            #region  生成权限 json
            rms_role Rmodel = SingleM(RoleId);
            Dictionary<string, Manu> list = GetRoleListManusD(RoleId);
            if (list == null)
            {
                list = new Dictionary<string, Manu>();
            }

            if (list != null)
            {
                Rmodel.jurisdiction = JsonHelper.ToJson(list, true);
                List<string> columns = new List<string>();//需要更新的列
                columns.Add("jurisdiction");
                int ies = Update(Rmodel, columns);//保存 权限json
                if (ies > 0)
                {
                    isSucces = true;
                }
            }
            else
            {
                isSucces = false;
            }
            #endregion
            return isSucces;
        }


        /// <summary>
        /// 从role  的json  根据角色 获取拥有的菜单 按钮等
        /// </summary>
        /// <param name="Jurisdiction">权限json字符串</param>
        /// <returns></returns>
        public Dictionary<string, Manu> GetRoleListManusD2(string Jurisdiction)
        {
            Dictionary<string, Manu> ListManusD = JsonHelper.FromJson<Dictionary<string, Manu>>(Jurisdiction);
            ListManusD = ListManusD.OrderBy(p => p.Value.manuinfo.orderno).ToDictionary(o => o.Key, p => p.Value);//排序
            foreach (var item in ListManusD)
            {
                item.Value.havebuttonsD = item.Value.havebuttonsD.OrderBy(p => p.Value.orderno).ToDictionary(o => o.Key, p => p.Value);//排序
            }
            return ListManusD;
        }
    }
}

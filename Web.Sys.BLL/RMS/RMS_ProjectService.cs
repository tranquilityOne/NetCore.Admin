using Core.PetaPocoServer;
using System;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_ProjectService : BaseDAL<rms_project>
    {
        #region 单例实现
        private static RMS_ProjectService singleInstance;

        private static object obj = new object();

        private RMS_ProjectService() { }

        public static RMS_ProjectService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_ProjectService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deleteProject(int id)
        {
            GetDatabase().BeginTransaction();//事务开始
            try
            {
                string sqlstr = " delete from  rms_rolemanubuttons where roleid in(select id from  rms_role where projectid=" + id + ")";//删除角色菜单按钮
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_rolemanus where roleid in(select id from  rms_role where projectid=" + id + ")";//删除角色菜单
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_menubuttons where manuid in (select id from  rms_menus where projectid=" + id + ")";//删除菜单按钮
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_role where projectid=" + id;//删除角色
                Execute(new Sql(sqlstr));
                sqlstr = " delete from  rms_menus where projectid=" + id;//删除菜单
                Execute(new Sql(sqlstr));

                int res = DeleteM(id);
                if (res > 0)
                {
                    GetDatabase().CompleteTransaction();//事务完成
                    return res;
                }
                else
                {

                    GetDatabase().AbortTransaction();//事务回滚
                    return 0;
                }

            }
            catch (Exception ex)
            {
                GetDatabase().AbortTransaction();//事务回滚
                return 0;
            }
        }
    }
}

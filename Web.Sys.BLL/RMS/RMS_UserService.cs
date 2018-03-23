using System;
using System.Collections.Generic;
using System.Text;
using Core.PetaPocoServer;
using Web.Sys.Model;

namespace Web.Sys.BLL
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_UserService : BaseDAL<rms_user>
    {
        #region 单例实现
        private static RMS_UserService singleInstance;

        private static object obj = new object();

        private RMS_UserService() { }

        public static RMS_UserService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_UserService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int deteUser(string id)
        {
            GetDatabase().BeginTransaction();//事务开始
            try
            {
                string sqlstr = " delete from  rms_userrole where userid='" + id + "'";//删除绑定的角色
                Execute(new Sql(sqlstr));
                int res = DeleteM(id);//删除用户

                GetDatabase().CompleteTransaction();//事务完成
                return res;
            }
            catch (Exception ex)
            {
                GetDatabase().AbortTransaction();//事务回滚
                return 0;
            }
        }
    }
}

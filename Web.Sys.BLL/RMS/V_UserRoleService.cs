using System;
using System.Collections.Generic;
using System.Text;
using Web.Sys.Model;
using Core.PetaPocoServer;

namespace Web.Sys.BLL
{
    [SingleDbFactory("PostgreSql_DB")]
    public class V_UserRoleService : BaseDAL<v_userrole>
    {
        #region 单例实现
        private static V_UserRoleService singleInstance;

        private static object obj = new object();

        private V_UserRoleService() { }

        public static V_UserRoleService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new V_UserRoleService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

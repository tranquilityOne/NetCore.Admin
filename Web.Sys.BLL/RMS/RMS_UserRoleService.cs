using System;
using System.Collections.Generic;
using System.Text;
using Core.PetaPocoServer;
using Web.Sys.Model;

namespace Web.Sys.BLL
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_UserRoleService : BaseDAL<rms_userrole>
    {
        #region 单例实现
        private static RMS_UserRoleService singleInstance;

        private static object obj = new object();

        private RMS_UserRoleService() {}

        public static RMS_UserRoleService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_UserRoleService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

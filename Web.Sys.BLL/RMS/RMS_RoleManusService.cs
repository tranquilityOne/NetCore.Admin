using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_RoleManusService : BaseDAL<rms_rolemanus>
    {
        #region 单例实现
        private static RMS_RoleManusService singleInstance;

        private static object obj = new object();

        private RMS_RoleManusService() { }

        public static RMS_RoleManusService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_RoleManusService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

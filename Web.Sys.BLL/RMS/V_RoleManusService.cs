using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class V_RoleManusService : BaseDAL<v_rolemanus>
    {
        #region 单例实现
        private static V_RoleManusService singleInstance;

        private static object obj = new object();

        private V_RoleManusService() { }

        public static V_RoleManusService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new V_RoleManusService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

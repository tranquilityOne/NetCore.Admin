using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class V_RoleManuButtonsService : BaseDAL<v_rolemanubuttons>
    {
        #region 单例实现
        private static V_RoleManuButtonsService singleInstance;

        private static object obj = new object();

        private V_RoleManuButtonsService() { }

        public static V_RoleManuButtonsService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new V_RoleManuButtonsService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

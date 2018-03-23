using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class V_RMS_MenusService : BaseDAL<v_rms_menus>
    {
        #region 单例实现
        private static V_RMS_MenusService singleInstance;

        private static object obj = new object();

        private V_RMS_MenusService() { }

        public static V_RMS_MenusService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new V_RMS_MenusService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

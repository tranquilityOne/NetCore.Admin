using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_RoleManuButtonsService : BaseDAL<rms_rolemanubuttons>
    {
        #region 单例实现
        private static RMS_RoleManuButtonsService singleInstance;

        private static object obj = new object();

        private RMS_RoleManuButtonsService() { }

        public static RMS_RoleManuButtonsService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_RoleManuButtonsService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

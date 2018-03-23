using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_MenuButtonsService : BaseDAL<rms_menubuttons>
    {
        #region 单例实现
        private static RMS_MenuButtonsService singleInstance;

        private static object obj = new object();

        private RMS_MenuButtonsService() { }

        public static RMS_MenuButtonsService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_MenuButtonsService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

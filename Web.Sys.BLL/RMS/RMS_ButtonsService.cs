using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_ButtonsService : BaseDAL<rms_buttons>
    {
        #region 单例实现
        private static RMS_ButtonsService singleInstance;

        private static object obj = new object();

        private RMS_ButtonsService() { }

        public static RMS_ButtonsService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_ButtonsService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}

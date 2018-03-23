using Core.PetaPocoServer;
using Web.Sys.Model.RMS;

namespace Web.Sys.BLL.RMS
{
    [SingleDbFactory("PostgreSql_DB")]
    public class RMS_CompanyService : BaseDAL<rms_company>
    {
        #region 单例实现
        private static RMS_CompanyService singleInstance;

        private static object obj = new object();

        private RMS_CompanyService() { }

        public static RMS_CompanyService Instance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (obj)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new RMS_CompanyService();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion
    }
}
